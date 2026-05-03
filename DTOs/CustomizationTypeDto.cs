namespace Tasty_Treat_be.DTOs
{
    public class CustomizationTypeDto
    {
        public int TypeId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CreateCustomizationTypeDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateCustomizationTypeDto
    {
        public string? Name { get; set; }
    }
}
