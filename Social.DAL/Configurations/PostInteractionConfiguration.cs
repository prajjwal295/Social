using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.DAL.Configurations
{
    internal class PostInteractionConfiguration : IEntityTypeConfiguration<PostInteraction>
    {
        public void Configure(EntityTypeBuilder<PostInteraction> builder)
        {
            builder.HasKey(pc => pc.InteractionId);
        }
    }
}
