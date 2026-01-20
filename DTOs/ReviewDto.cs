namespace Tasty_Treat_be.DTOs
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int CustomerId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
    }

    public class CreateReviewDto
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int CustomerId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
    }

    public class UpdateReviewDto
    {
        public string? Comment { get; set; }
        public int? Rating { get; set; }
    }
}
