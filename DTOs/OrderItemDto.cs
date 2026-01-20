namespace Tasty_Treat_be.DTOs
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int? CustomizationOptionId { get; set; }
        public int Quantity { get; set; }
        public decimal OrderItemPrice { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateOrderItemDto
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int? CustomizationOptionId { get; set; }
        public int Quantity { get; set; }
        public decimal OrderItemPrice { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class UpdateOrderItemDto
    {
        public int? Quantity { get; set; }
        public decimal? OrderItemPrice { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
