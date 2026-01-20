using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasty_Treat_be.Models
{
    [Table("Delivery")]
    public class Delivery
    {
        [Key]
        [Column("delivery_id")]
        public int DeliveryId { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("delivery_person_id")]
        public int DeliveryPersonId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("delivery_status")]
        public string DeliveryStatus { get; set; } = string.Empty;

        [Column("delivery_notes")]
        public string? DeliveryNotes { get; set; }

        [Column("assigned_at")]
        public DateTime? AssignedAt { get; set; }

        [Column("picked_up_at")]
        public DateTime? PickedUpAt { get; set; }

        [Column("delivered_at")]
        public DateTime? DeliveredAt { get; set; }

        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [ForeignKey("DeliveryPersonId")]
        public virtual User? DeliveryPerson { get; set; }
    }
}
