using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasty_Treat_be.Models
{
    [Table("Customization_option")]
    public class CustomizationOption
    {
        [Key]
        [Column("option_id")]
        public int OptionId { get; set; }

        [Column("item_id")]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;

        [Column("additional_price", TypeName = "decimal(10,2)")]
        public decimal AdditionalPrice { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
