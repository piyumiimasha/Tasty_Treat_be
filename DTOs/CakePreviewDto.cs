namespace Tasty_Treat_be.DTOs
{
    public class GenerateCakePreviewDto
    {
        public List<int> SelectedOptionIds { get; set; } = new();
        public string? Instructions { get; set; }
    }

    public class CakePreviewResponseDto
    {
        public string ImageUrl { get; set; } = string.Empty;
    }
}
