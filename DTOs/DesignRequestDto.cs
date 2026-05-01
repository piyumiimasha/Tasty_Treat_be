namespace Tasty_Treat_be.DTOs
{
    public class DesignRequestDto
    {
        public int DesignRequestId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? Message { get; set; }
        public string? ImageUrl { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime CreatedAt { get; set; }
    }

    public class CreateDesignRequestDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string? Message { get; set; }
    }

    public class UpdateDesignRequestDto
    {
        public string? Status { get; set; }
    }
}
