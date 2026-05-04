using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasty_Treat_be.Models
{
    [Table("Design_Request")]
    public class DesignRequest
    {
        [Key]
        [Column("design_request_id")]
        public int DesignRequestId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("customer_name")]
        public string CustomerName { get; set; } = string.Empty;

        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [Column("message")]
        public string? Message { get; set; }

        [MaxLength(500)]
        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "pending";

        [Column("quoted_price", TypeName = "decimal(10,2)")]
        public decimal? QuotedPrice { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
