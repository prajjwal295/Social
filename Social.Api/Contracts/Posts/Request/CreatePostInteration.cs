using Social.Domain.Aggregates.PostAggregate;

namespace Social.Api.Contracts.Posts.Request
{
    public class CreatePostInteration
    {
        public InteractionType InteractionType { get; set; }
    }
}
