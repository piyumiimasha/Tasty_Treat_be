using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasty_Treat_be.Models
{
    [Table("Item_Flavour")]
    public class ItemFlavour
    {
        [Key]
        [Column("item_flavour_id")]
        public int ItemFlavourId { get; set; }

        [Column("item_id")]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column("extra_price", TypeName = "decimal(10,2)")]
        public decimal ExtraPrice { get; set; }

        public virtual Item Item { get; set; } = null!;
    }
}
