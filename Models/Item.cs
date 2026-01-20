using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasty_Treat_be.Models
{
    [Table("Item")]
    public class Item
    {
        [Key]
        [Column("item_id")]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Column("base_price", TypeName = "decimal(10,2)")]
        public decimal BasePrice { get; set; }

        [MaxLength(20)]
        [Column("base_price_unit")]
        public string? BasePriceUnit { get; set; }

        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<CustomizationOption> CustomizationOptions { get; set; } = new List<CustomizationOption>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
