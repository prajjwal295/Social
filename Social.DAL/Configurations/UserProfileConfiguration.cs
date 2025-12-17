using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social.Domain.Aggregates.UserProfileAggegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Social.DAL.Configurations
{
    internal class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.OwnsOne(up => up.BasicInfo , basicInfo =>
            {
                basicInfo.Property(b => b.FirstName)
                .HasMaxLength(100);

                basicInfo.Property(b => b.LastName)
                .HasMaxLength(100);

                basicInfo.Property(b => b.EmailAddress)
                .HasMaxLength(100);

                basicInfo.Property(b => b.Phone)
                .HasMaxLength(20);

                basicInfo.Property(b => b.CurrentCity)
                .HasMaxLength(100);
            });

               builder
                .HasMany(u => u.RefreshToken)
                .WithOne(r => r.UserProfile)
                .HasForeignKey(r => r.UserProfileId);
        }
    }
}
