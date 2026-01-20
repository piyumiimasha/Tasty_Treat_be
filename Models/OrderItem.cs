using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasty_Treat_be.Models
{
    [Table("Order_Item")]
    public class OrderItem
    {
        [Key]
        [Column("order_item_id")]
        public int OrderItemId { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("item_id")]
        public int ItemId { get; set; }

        [Column("customization_option_id")]
        public int? CustomizationOptionId { get; set; }

        public int Quantity { get; set; }

        [Column("order_item_price", TypeName = "decimal(10,2)")]
        public decimal OrderItemPrice { get; set; }

        [Column("is_available")]
        public bool IsAvailable { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        [ForeignKey("CustomizationOptionId")]
        public virtual CustomizationOption? CustomizationOption { get; set; }
    }
}
