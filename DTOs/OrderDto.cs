namespace Tasty_Treat_be.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? DeliveryAddress { get; set; }
        public string? SpecialInstructions { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public string Status { get; set; } = "Pending";
        public string? DeliveryAddress { get; set; }
        public string? SpecialInstructions { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class UpdateOrderDto
    {
        public string? Status { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? SpecialInstructions { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
