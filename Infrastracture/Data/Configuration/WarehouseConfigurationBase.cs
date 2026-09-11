using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Projekt.Infrastructure.Data.Configuration
{
    public abstract class WarehouseConfigurationBase : IEntityTypeConfiguration<Warehouse>
    {
        public virtual void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("warehouse");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.HasOne(w => w.Address)
                   .WithMany()
                   .HasForeignKey(w => w.AddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.Storages)
                   .WithOne(s => s.Warehouse)
                   .HasForeignKey(s => s.WarehouseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
