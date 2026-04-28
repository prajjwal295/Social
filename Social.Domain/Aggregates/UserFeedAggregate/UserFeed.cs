using System;
using System.Collections.Generic;

namespace Social.Domain.Aggregates.UserFeedAggregate
{
    public class UserFeed
    {
        private readonly List<FeedItem> _feedItems = new();
        private const int MAX_FEED_ITEMS = 500;

        private UserFeed() { }

        public Guid UserFeedId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public IEnumerable<FeedItem> FeedItems => _feedItems;
        public DateTime UpdatedAt { get; private set; }

        public static UserFeed Create(Guid userId)
        {
            return new UserFeed
            {
                UserFeedId = Guid.NewGuid(),
                UserProfileId = userId,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public void AddPost(Guid postId, DateTime createdAt)
        {
            if (_feedItems.Any(f => f.PostId == postId))
                return;

            var item = FeedItem.Create(postId, createdAt);

            _feedItems.Insert(0, item);

            if (_feedItems.Count > MAX_FEED_ITEMS)
            {
                _feedItems.RemoveAt(_feedItems.Count - 1);
            }

            UpdatedAt = DateTime.UtcNow;
        }

        public void RemovePost(Guid postId)
        {
            var item = _feedItems.FirstOrDefault(f => f.PostId == postId);
            if (item != null)
            {
                _feedItems.Remove(item);
                UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}