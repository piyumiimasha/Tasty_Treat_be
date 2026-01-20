using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasty_Treat_be.Models
{
    [Table("Instant_Quote")]
    public class InstantQuote
    {
        [Key]
        [Column("quote_id")]
        public int QuoteId { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("Converted_order_id")]
        public int? ConvertedOrderId { get; set; }

        // Storing items as JSON string (ArrayList<Item> representation)
        [Required]
        public string Items { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Tax { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Discount { get; set; }

        [Column("delivery_fee")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal DeliveryFee { get; set; }

        [Column("estimated_price")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal EstimatedPrice { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual User? Customer { get; set; }

        [ForeignKey("ConvertedOrderId")]
        public virtual Order? ConvertedOrder { get; set; }
    }
}
