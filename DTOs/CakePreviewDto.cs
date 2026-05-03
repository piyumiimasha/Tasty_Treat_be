namespace Tasty_Treat_be.DTOs
{
    public class GenerateCakePreviewDto
    {
        public int? LayersOptionId { get; set; }
        public int? ShapeOptionId { get; set; }
        public int? FrostingOptionId { get; set; }
        public int? FlavourOptionId { get; set; }
        public int? TopperOptionId { get; set; }
        public int? ColorOptionId { get; set; }
        public List<int> DecorationOptionIds { get; set; } = new();
        public List<int> DietaryOptionIds { get; set; } = new();
        public string? Instructions { get; set; }
    }

    public class CakePreviewResponseDto
    {
        public string ImageUrl { get; set; } = string.Empty;
    }
}
