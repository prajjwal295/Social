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

            builder.HasMany(x => x.FeedItems)
                .WithOne()
                .HasForeignKey(x => x.UserFeedId);
        }
    }
}
