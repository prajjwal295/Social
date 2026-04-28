
namespace Social.Application.DTO
{
    public class FeedDto
    {
        public Guid PostId { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Source { get; set; }
    }
}
