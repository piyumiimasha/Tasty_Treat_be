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
            var color       = Get("colors", "color");
            var decorations = GetMany("decorations");
            var dietary     = GetMany("dietary");

            // Any custom types beyond the standard set go into a generic description
            var standardTypes = new HashSet<string>
                { "layers", "shapes", "shape", "frosting", "flavour", "flavours",
                  "toppers", "topper", "colors", "color", "decorations", "dietary" };
            var extras = selected
                .Where(o => !standardTypes.Contains((o.CustomizationType?.Name ?? "").ToLower()))
                .Select(o => o.Name)
                .ToList();

            var layerDisplay  = string.IsNullOrEmpty(layers)  ? "1"           : layers;
            var shapeDisplay  = string.IsNullOrEmpty(shape)   ? "round"       : shape;
            var frostDisplay  = string.IsNullOrEmpty(frosting) ? "buttercream" : frosting;
            var flavourDisplay = string.IsNullOrEmpty(flavour) ? "vanilla"     : flavour;

            var topperStr  = !string.IsNullOrEmpty(topper) && topper.ToLower() != "none"
                             ? $", decorated with {topper}" : "";
            var colorStr   = !string.IsNullOrEmpty(color)       ? $", in {color} color theme" : "";
            var decorStr   = decorations.Length > 0 ? $", with {string.Join(" and ", decorations)} decorations" : "";
            var dietaryStr = dietary.Length > 0     ? $", {string.Join(" and ", dietary)} friendly" : "";
            var extraStr   = extras.Count > 0       ? $", {string.Join(", ", extras)}" : "";
            var instrStr   = !string.IsNullOrEmpty(dto.Instructions) ? $". Additional details: {dto.Instructions}" : "";

            var prompt =
                $"A {layerDisplay}-layer {shapeDisplay}-shaped cake with {frostDisplay} frosting and {flavourDisplay} flavour" +
                $"{topperStr}{colorStr}{decorStr}{dietaryStr}{extraStr}{instrStr}. " +
                "Simple bakery-style cake, natural imperfections, smooth but not perfect frosting, realistic texture. " +
                "Placed on a cake board or simple stand, soft natural lighting, minimal shadows, neutral background. " +
                "Realistic food photography, not stylized, not CGI, not overly glossy.";

            var http = _httpClientFactory.CreateClient();
            http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _pixazoApiKey);
            http.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

            var body      = JsonSerializer.Serialize(new { prompt });
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
