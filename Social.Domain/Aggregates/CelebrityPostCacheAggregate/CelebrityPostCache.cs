using System.ComponentModel.DataAnnotations;

namespace Social.Domain.Aggregates.CelebrityPostCacheAggregate
{
    public class CelebrityPostCache
    {
        private CelebrityPostCache() { }

        [Key]
        public Guid CelebrityPostId { get; private set; }
        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public static CelebrityPostCache Create(Guid postId, Guid userId, DateTime createdAt)
        {
            if (postId == Guid.Empty || userId == Guid.Empty)
                throw new ArgumentException("PostId OR UserId cannot be empty");

            return new CelebrityPostCache
            {
                PostId = postId,
                UserProfileId = userId,
                CreatedAt = createdAt,
                CelebrityPostId = Guid.NewGuid()
            };
        }
    }
}
