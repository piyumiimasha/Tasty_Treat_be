using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CakePreviewController : ControllerBase
    {
        private readonly ICustomizationOptionRepository _optionRepo;
        private readonly IBlobStorageService _blobService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _pixazoApiKey;
        private readonly string _container;

        private const string PIXAZO_URL = "https://gateway.pixazo.ai/getImage/v1/getSDXLImage";

        public CakePreviewController(
            ICustomizationOptionRepository optionRepo,
            IBlobStorageService blobService,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _optionRepo        = optionRepo;
            _blobService       = blobService;
            _httpClientFactory = httpClientFactory;
            _pixazoApiKey      = configuration["PixazoApiKey"] ?? string.Empty;
            _container         = configuration["AzureBlobStorage:AiGeneratedContainer"] ?? "aigenerated";
        }

        [HttpPost("generate")]
        public async Task<ActionResult<CakePreviewResponseDto>> Generate([FromBody] GenerateCakePreviewDto dto)
        {
            if (string.IsNullOrEmpty(_pixazoApiKey))
                return BadRequest("AI preview not configured on the server.");

            var allOptions = await _optionRepo.GetAllAsync();
            var selected   = allOptions.Where(o => dto.SelectedOptionIds.Contains(o.OptionId)).ToList();

            // Group selected options by their type name (lowercase)
            var byType = selected
                .GroupBy(o => (o.CustomizationType?.Name ?? "other").ToLower())
                .ToDictionary(g => g.Key, g => g.Select(o => o.Name).ToList());

            string Get(params string[] keys)
            {
                foreach (var k in keys)
                    if (byType.TryGetValue(k, out var vals) && vals.Count > 0) return vals[0];
                return string.Empty;
            }
            string[] GetMany(string key) =>
                byType.TryGetValue(key, out var vals) ? vals.ToArray() : Array.Empty<string>();

            var layers      = Get("layers");
            var shape       = Get("shapes", "shape");
            var frosting    = Get("frosting");
            var flavour     = Get("flavour", "flavours");
            var topper      = Get("toppers", "topper");
            var fillings    = GetMany("fillings");
            var piping      = Get("piping");
            var color       = Get("colors", "color");
            var decorations = GetMany("decorations");
            var dietary     = GetMany("dietary");

            // Any custom types beyond the ones handled explicitly above fall through
            // to a generic clause that keeps the type name for context (e.g. a future
            // "Accents" type with value "Gold" reads as "gold accents").
            var standardTypes = new HashSet<string>
                { "layers", "shapes", "shape", "frosting", "flavour", "flavours",
                  "toppers", "topper", "fillings", "piping",
                  "colors", "color", "decorations", "dietary" };
            var extras = selected
                .Where(o => !standardTypes.Contains((o.CustomizationType?.Name ?? "").ToLower()))
                .Select(o => $"{o.Name} {o.CustomizationType?.Name}".Trim().ToLower())
                .ToList();

            // SDXL's CLIP encoder weights early tokens most and truncates around 77
            // tokens, so the prompt leads with the cake form, then the customer's own
            // instructions (highest intent), then the selected options, and keeps the
            // realism cues short so nothing important gets cut off.
            bool Has(string? v) => !string.IsNullOrWhiteSpace(v) && !v.Trim().Equals("none", StringComparison.OrdinalIgnoreCase);

            var sb = new StringBuilder("A ");
            if (Has(layers)) sb.Append($"{layers.Trim()}-tier ");
            sb.Append(Has(shape) ? $"{shape.Trim()} cake" : "round cake");

            if (Has(dto.Instructions))
                sb.Append($", {dto.Instructions!.Trim().TrimEnd('.')}");

            if (Has(frosting)) sb.Append($", covered in {frosting.Trim()} frosting");
            if (Has(flavour))  sb.Append($", {flavour.Trim()} flavoured");
            if (fillings.Length > 0)    sb.Append($", filled with {string.Join(" and ", fillings)}");
            if (Has(color))    sb.Append($", {color.Trim()} color theme");
            if (decorations.Length > 0) sb.Append($", decorated with {string.Join(" and ", decorations)}");
            if (Has(piping))   sb.Append($", {piping.Trim()} piping detail");
            if (Has(topper))   sb.Append($", topped with {topper.Trim()}");
            if (dietary.Length > 0)     sb.Append($", {string.Join(" and ", dietary)} friendly");
            if (extras.Count > 0)       sb.Append($", {string.Join(", ", extras)}");

            sb.Append(". Realistic professional bakery cake on a simple stand, ");
            sb.Append("appetizing food photography, soft natural lighting, neutral background, highly detailed, sharp focus.");

            var prompt = sb.ToString();

            const string negativePrompt =
                "cartoon, illustration, drawing, painting, sketch, 3d render, cgi, anime, " +
                "plastic, overly glossy, deformed, distorted, blurry, low quality, grainy, " +
                "watermark, text, signature, extra cakes, melted, messy, ugly, unappetizing";

            var http = _httpClientFactory.CreateClient();
            http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _pixazoApiKey);
            http.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

            var body      = JsonSerializer.Serialize(new { prompt, negativePrompt });
            var pixazoRes = await http.PostAsync(PIXAZO_URL,
                new StringContent(body, Encoding.UTF8, "application/json"));

            if (!pixazoRes.IsSuccessStatusCode)
            {
                var err = await pixazoRes.Content.ReadAsStringAsync();
                return StatusCode(502, $"Pixazo error: {err}");
            }

            var pixazoJson = await pixazoRes.Content.ReadAsStringAsync();
            using var doc  = JsonDocument.Parse(pixazoJson);
            var root       = doc.RootElement;

            string? pixazoImageUrl = null;
            if (root.TryGetProperty("imageUrl",  out var p1)) pixazoImageUrl = p1.GetString();
            else if (root.TryGetProperty("image_url", out var p2)) pixazoImageUrl = p2.GetString();
            else if (root.TryGetProperty("url",   out var p3)) pixazoImageUrl = p3.GetString();

            if (string.IsNullOrEmpty(pixazoImageUrl))
                return StatusCode(502, $"Unexpected Pixazo response: {pixazoJson}");

            var imageBytes = await http.GetByteArrayAsync(pixazoImageUrl);
            using var stream = new MemoryStream(imageBytes);

            var blobUrl = await _blobService.UploadFromStreamAsync(
                stream, "cake-preview.jpg", "image/jpeg", _container);

            return Ok(new CakePreviewResponseDto { ImageUrl = blobUrl });
        }
    }
}
