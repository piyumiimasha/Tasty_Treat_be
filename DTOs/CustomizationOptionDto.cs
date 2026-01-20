namespace Tasty_Treat_be.DTOs
{
    public class CustomizationOptionDto
    {
        public int OptionId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal AdditionalPrice { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateCustomizationOptionDto
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal AdditionalPrice { get; set; }
    }

    public class UpdateCustomizationOptionDto
    {
        public int? ItemId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public decimal? AdditionalPrice { get; set; }
    }
}
