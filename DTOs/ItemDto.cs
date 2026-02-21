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
        public string? Flavour { get; set; }
        public string? ImageUrl { get; set; }
    }

    /// <summary>
    /// DTO for creating a new item (e.g., cake, dessert)
    /// Note: Images are uploaded separately via the Create endpoint's 'image' parameter
    /// </summary>
    public class CreateItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string? BasePriceUnit { get; set; }
        public string? Description { get; set; }
        public string? Flavour { get; set; }
    }

    /// <summary>
    /// DTO for updating an existing item
    /// Note: Images are updated separately via the Update endpoint's 'image' parameter
    /// </summary>
    public class UpdateItemDto
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public decimal? BasePrice { get; set; }
        public string? BasePriceUnit { get; set; }
        public string? Description { get; set; }
        public string? Flavour { get; set; }
    }
}
