using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Projekt.Infrastructure.Data.Configuration
{
    public abstract class PackageConfigurationBase : IEntityTypeConfiguration<Package>
    {
        public virtual void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.ToTable("package");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Status)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.HasOne(p => p.PickupAddress)
                   .WithMany()
                   .HasForeignKey(p => p.PickupAddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.DeliveryAddress)
                   .WithMany()
                   .HasForeignKey(p => p.DeliveryAddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.StorageAddress)
                   .WithMany()
                   .HasForeignKey(p => p.StorageAddressId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

