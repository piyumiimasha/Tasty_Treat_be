namespace Tasty_Treat_be.DTOs
{
    public class ItemFlavourDto
    {
        public int ItemFlavourId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal ExtraPrice { get; set; }
    }

    public class CreateItemFlavourDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal ExtraPrice { get; set; }
    }

    public class UpdateItemFlavourDto
    {
        public string? Name { get; set; }
        public decimal? ExtraPrice { get; set; }
    }
}
