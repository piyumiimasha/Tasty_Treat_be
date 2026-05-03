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
            _optionRepo    = optionRepo;
            _blobService   = blobService;
            _httpClientFactory = httpClientFactory;
            _pixazoApiKey  = configuration["PixazoApiKey"] ?? string.Empty;
            _container     = configuration["AzureBlobStorage:AiGeneratedContainer"] ?? "aigenerated";
        }

        [HttpPost("generate")]
        public async Task<ActionResult<CakePreviewResponseDto>> Generate([FromBody] GenerateCakePreviewDto dto)
        {
            if (string.IsNullOrEmpty(_pixazoApiKey))
                return BadRequest("AI preview not configured on the server.");

            // Collect all referenced option IDs
            var allIds = new List<int?> {
                dto.LayersOptionId, dto.ShapeOptionId, dto.FrostingOptionId,
                dto.FlavourOptionId, dto.TopperOptionId, dto.ColorOptionId
            }
            .Where(id => id.HasValue).Select(id => id!.Value)
            .Concat(dto.DecorationOptionIds)
            .Concat(dto.DietaryOptionIds)
            .Distinct()
            .ToList();

            // Fetch option names from DB
            var allOptions = await _optionRepo.GetAllAsync();
            var nameMap = allOptions
                .Where(o => allIds.Contains(o.OptionId))
                .ToDictionary(o => o.OptionId, o => o.Name);

            string Resolve(int? id) => id.HasValue && nameMap.TryGetValue(id.Value, out var n) ? n : string.Empty;
            string[] ResolveMany(List<int> ids) => ids.Select(id => nameMap.TryGetValue(id, out var n) ? n : string.Empty).Where(n => n.Length > 0).ToArray();

            var layers       = Resolve(dto.LayersOptionId);
            var shape        = Resolve(dto.ShapeOptionId);
            var frosting     = Resolve(dto.FrostingOptionId);
            var flavour      = Resolve(dto.FlavourOptionId);
            var topper       = Resolve(dto.TopperOptionId);
            var color        = Resolve(dto.ColorOptionId);
            var decorations  = ResolveMany(dto.DecorationOptionIds);
            var dietary      = ResolveMany(dto.DietaryOptionIds);

            // Build prompt
            var topperStr     = !string.IsNullOrEmpty(topper) && topper.ToLower() != "none" ? $", decorated with {topper}" : "";
            var colorStr      = !string.IsNullOrEmpty(color) ? $", in {color} color theme" : "";
            var decorStr      = decorations.Length > 0 ? $", with {string.Join(" and ", decorations)} decorations" : "";
            var dietaryStr    = dietary.Length > 0 ? $", {string.Join(" and ", dietary)} friendly" : "";
            var instrStr      = !string.IsNullOrEmpty(dto.Instructions) ? $". Additional details: {dto.Instructions}" : "";
            var layerDisplay  = string.IsNullOrEmpty(layers) ? "1" : layers;
            var shapeDisplay  = string.IsNullOrEmpty(shape)  ? "round" : shape;
            var frostDisplay  = string.IsNullOrEmpty(frosting) ? "buttercream" : frosting;
            var flavourDisplay = string.IsNullOrEmpty(flavour) ? "vanilla" : flavour;

            var prompt =
            $"A {layerDisplay}-layer {shapeDisplay}-shaped cake with {frostDisplay} frosting and {flavourDisplay} flavour" +
            $"{topperStr}{colorStr}{decorStr}{dietaryStr}{instrStr}. " +
            "Simple bakery-style cake, natural imperfections, smooth but not perfect frosting, realistic texture. " +
            "Placed on a cake board or simple stand, soft natural lighting, minimal shadows, neutral background. " +
            "Realistic food photography, not stylized, not CGI, not overly glossy.";

            var http = _httpClientFactory.CreateClient();
            http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _pixazoApiKey);
            http.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

            // Call Pixazo
            var body = JsonSerializer.Serialize(new { prompt });
            var pixazoRes = await http.PostAsync(PIXAZO_URL,
                new StringContent(body, Encoding.UTF8, "application/json"));

            if (!pixazoRes.IsSuccessStatusCode)
            {
                var err = await pixazoRes.Content.ReadAsStringAsync();
                return StatusCode(502, $"Pixazo error: {err}");
            }

            var pixazoJson = await pixazoRes.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(pixazoJson);
            var root = doc.RootElement;

            string? pixazoImageUrl = null;
            if (root.TryGetProperty("imageUrl",  out var p1)) pixazoImageUrl = p1.GetString();
            else if (root.TryGetProperty("image_url", out var p2)) pixazoImageUrl = p2.GetString();
            else if (root.TryGetProperty("url",   out var p3)) pixazoImageUrl = p3.GetString();

            if (string.IsNullOrEmpty(pixazoImageUrl))
                return StatusCode(502, $"Unexpected Pixazo response: {pixazoJson}");

            // Download image from Pixazo and upload to Azure Blob
            var imageBytes = await http.GetByteArrayAsync(pixazoImageUrl);
            using var stream = new MemoryStream(imageBytes);

            var blobUrl = await _blobService.UploadFromStreamAsync(
                stream, "cake-preview.jpg", "image/jpeg", _container);

            return Ok(new CakePreviewResponseDto { ImageUrl = blobUrl });
        }
    }
}
