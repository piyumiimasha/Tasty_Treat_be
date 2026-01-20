namespace Tasty_Treat_be.DTOs
{
    public class InstantQuoteDto
    {
        public int QuoteId { get; set; }
        public int CustomerId { get; set; }
        public int? ConvertedOrderId { get; set; }
        public string Items { get; set; } = string.Empty;
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal EstimatedPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateInstantQuoteDto
    {
        public int CustomerId { get; set; }
        public string Items { get; set; } = string.Empty;
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal EstimatedPrice { get; set; }
    }

    public class UpdateInstantQuoteDto
    {
        public int? ConvertedOrderId { get; set; }
        public string? Items { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DeliveryFee { get; set; }
        public decimal? EstimatedPrice { get; set; }
    }
}
