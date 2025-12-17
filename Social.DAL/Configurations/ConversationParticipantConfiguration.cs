using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Social.Domain.Aggregates.ConversationAggreagate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.DAL.Configurations
{
    internal class ConversationParticipantConfiguration : IEntityTypeConfiguration<ConversationParticipant>
    {
        public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
        {
            builder.HasNoKey();
        }
    }
}
