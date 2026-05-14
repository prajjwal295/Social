using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Social.DAL.Configurations;
using Social.DAL.DbContext.Interceptors;
using Social.DAL.Migrations;
using Social.Domain.Aggregates.CelebrityPostCacheAggregate;
using Social.Domain.Aggregates.ConversationAggreagate;
using Social.Domain.Aggregates.FollowAggregate.Social.Domain.Aggregates.FollowAggregate;
using Social.Domain.Aggregates.PostAggregate;
using Social.Domain.Aggregates.UserFeedAggregate;
using Social.Domain.Aggregates.UserProfileAggegate;

namespace Social.DAL.DbContext
{
    public class DataContext : IdentityDbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _interceptor;

        public DataContext(DbContextOptions options , AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        {
            _interceptor= auditableEntitySaveChangesInterceptor;
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComment { get; set; }
        public DbSet<PostInteraction> PostInteraction { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationParticipant> ConversationParticipants { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserFeed> UserFeed { get; set; }
        public DbSet<FeedItem> FeedItems { get; set; }
        public DbSet<Follow> Followers { get; set; }
        public DbSet<Domain.Aggregates.CelebrityPostCacheAggregate.CelebrityPostCache> CelebrityPostCache { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PostCommentConfiguration());
            builder.ApplyConfiguration(new PostInteractionConfiguration());
            builder.ApplyConfiguration(new IdentityUserLoginConfiguration());
            builder.ApplyConfiguration(new UserProfileConfiguration());
            builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
            builder.ApplyConfiguration(new IdentityUserTokenConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());
            builder.ApplyConfiguration(new UserFeedConfiguration());
            builder.ApplyConfiguration(new FeedItemConfiguration());
            builder.ApplyConfiguration(new FollowConfiguration());
            //builder.ApplyConfiguration(new ConversationParticipantConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_interceptor);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
