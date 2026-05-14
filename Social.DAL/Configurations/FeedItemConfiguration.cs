using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social.Domain.Aggregates.UserFeedAggregate;

namespace Social.DAL.Configurations
{
    internal class FeedItemConfiguration : IEntityTypeConfiguration<FeedItem>
    {
        public void Configure(EntityTypeBuilder<FeedItem> builder)
        {
            builder.ToTable("UserFeedItems");

            builder.HasKey(x => new
            {
                x.UserFeedId,
                x.PostId
            });

            builder.Property(x => x.CreatedAt)
                .IsRequired();
        }
    }
}