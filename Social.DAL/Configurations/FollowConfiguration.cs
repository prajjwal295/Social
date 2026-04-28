using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social.Domain.Aggregates.FollowAggregate.Social.Domain.Aggregates.FollowAggregate;

namespace Social.DAL.Configurations
{
    internal class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.HasKey(x => x.FollowId);

            builder.HasIndex(x => new { x.FollowerId, x.FolloweeId })
                .IsUnique();
        }
    }
}
