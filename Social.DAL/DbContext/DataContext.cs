using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Social.DAL.Configurations;
using Social.DAL.DbContext.Interceptors;
using Social.Domain.Aggregates.ConversationAggreagate;
using Social.Domain.Aggregates.PostAggregate;
using Social.Domain.Aggregates.UserProfileAggegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PostCommentConfiguration());
            builder.ApplyConfiguration(new PostInteractionConfiguration());
            builder.ApplyConfiguration(new IdentityUserLoginConfiguration());
            builder.ApplyConfiguration(new UserProfileConfiguration());
            builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
            builder.ApplyConfiguration(new IdentityUserTokenConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());
            //builder.ApplyConfiguration(new ConversationParticipantConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_interceptor);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
