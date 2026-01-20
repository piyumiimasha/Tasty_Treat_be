namespace Tasty_Treat_be.DTOs
{
    public class DeliveryDto
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
        public int DeliveryPersonId { get; set; }
        public string DeliveryStatus { get; set; } = string.Empty;
        public string? DeliveryNotes { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? PickedUpAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }

    public class CreateDeliveryDto
    {
        public int OrderId { get; set; }
        public int DeliveryPersonId { get; set; }
        public string DeliveryStatus { get; set; } = "Pending";
        public string? DeliveryNotes { get; set; }
    }

    public class UpdateDeliveryDto
    {
        public int? DeliveryPersonId { get; set; }
        public string? DeliveryStatus { get; set; }
        public string? DeliveryNotes { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? PickedUpAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
