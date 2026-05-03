namespace Tasty_Treat_be.DTOs
{
    public class CustomizationOptionDto
    {
        public int OptionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public decimal AdditionalPrice { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateCustomizationOptionDto
    {
        public string Name { get; set; } = string.Empty;
        public int TypeId { get; set; }
        public decimal AdditionalPrice { get; set; }
    }

    public class UpdateCustomizationOptionDto
    {
        public string? Name { get; set; }
        public int? TypeId { get; set; }
        public decimal? AdditionalPrice { get; set; }
    }
}
