using Social.Domain.Aggregates.PostAggregate;

namespace Social.Api.Contracts.Posts.Response
{
    public class PostInteractionResponse
    {
        public Guid InteractionId { get; set; }
        public InteractionType Interaction {  get; set; }
        public Guid postId { get; set; }
        public Guid userProfileId { get; set; }
    }
}
