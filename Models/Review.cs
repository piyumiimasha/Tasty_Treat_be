using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasty_Treat_be.Models
{
    [Table("Review")]
    public class Review
    {
        [Key]
        [Column("review_id")]
        public int ReviewId { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("item_id")]
        public int ItemId { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        public string? Comment { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Column("review_date")]
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        [ForeignKey("CustomerId")]
        public virtual User? Customer { get; set; }
    }
}
