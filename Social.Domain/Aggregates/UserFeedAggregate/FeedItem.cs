using System;

namespace Social.Domain.Aggregates.UserFeedAggregate
{
    public class FeedItem
    {
        private FeedItem() { }

        public Guid UserFeedId { get; private set; }
        public Guid PostId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public static FeedItem Create(Guid postId, DateTime createdAt)
        {
            if (postId == Guid.Empty)
                throw new ArgumentException("PostId cannot be empty");

            return new FeedItem
            {
                PostId = postId,
                CreatedAt = createdAt
            };
        }
    }
}