namespace Social.Domain.Aggregates.FollowAggregate
{
    namespace Social.Domain.Aggregates.FollowAggregate
    {
        public class Follow
        {
            private Follow() { }

            public Guid FollowId { get; private set; }

            public Guid FollowerId { get; private set; }
            public Guid FolloweeId { get; private set; }

            public DateTime CreatedAt { get; private set; }
            public DateTime? UnfollowedAt { get; private set; }

            public bool IsActive => UnfollowedAt == null;

            public static Follow Create(Guid followerId, Guid followeeId)
            {
                if (followerId == followeeId)
                    throw new Exception("Cannot follow yourself");

                return new Follow
                {
                    FollowId = Guid.NewGuid(),
                    FollowerId = followerId,
                    FolloweeId = followeeId,
                    CreatedAt = DateTime.UtcNow,
                    UnfollowedAt = null
                };
            }

            public void Reactivate()
            {
                UnfollowedAt = null;
            }

            public void Unfollow()
            {
                if (UnfollowedAt != null)
                    return;

                UnfollowedAt = DateTime.UtcNow;
            }
        }
    }
}
