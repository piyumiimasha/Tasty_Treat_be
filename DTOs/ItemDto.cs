namespace Tasty_Treat_be.DTOs
{
    public class ItemDto
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string? BasePriceUnit { get; set; }
        public string? Description { get; set; }
    }

    public class CreateItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string? BasePriceUnit { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateItemDto
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public decimal? BasePrice { get; set; }
        public string? BasePriceUnit { get; set; }
        public string? Description { get; set; }
    }
}
