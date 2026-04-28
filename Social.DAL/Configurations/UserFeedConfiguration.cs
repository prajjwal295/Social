using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social.Domain.Aggregates.UserFeedAggregate;

namespace Social.DAL.Configurations
{
    internal class UserFeedConfiguration : IEntityTypeConfiguration<UserFeed>
    {
        public void Configure(EntityTypeBuilder<UserFeed> builder)
        {
            builder.HasKey(x => x.UserFeedId);

            builder.OwnsMany(x => x.FeedItems, fb =>
            {
                fb.WithOwner().HasForeignKey(x => x.UserFeedId);

                fb.HasKey(x => new { x.UserFeedId, x.PostId });

                fb.Property(x => x.CreatedAt).IsRequired();

                fb.ToTable("UserFeedItems");
            });
        }
    }
}
