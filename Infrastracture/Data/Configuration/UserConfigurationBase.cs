using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Infrastructure.Identity;

namespace Projekt.Infrastructure.Data.Configuration
{
    public abstract class UserConfigurationBase : IEntityTypeConfiguration<ApplicationUser>
    {
        public virtual void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("user");

            builder.Property(u => u.UserName).HasMaxLength(256);
            builder.Property(u => u.Email).HasMaxLength(256);

            builder.HasOne<Domain.Entities.Warehouse>()
                   .WithMany()
                   .HasForeignKey(u => u.WorkplaceId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

