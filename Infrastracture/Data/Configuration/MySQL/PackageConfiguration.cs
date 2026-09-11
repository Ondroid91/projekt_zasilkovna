using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Infrastructure.Data.Configuration;

namespace Projekt.Infrastructure.Data.Configuration.MySQL
{
    public class PackageConfiguration : PackageConfigurationBase
    {
        public override void Configure(EntityTypeBuilder<Domain.Entities.Package> builder)
        {
            base.Configure(builder);

            builder.HasOne(p => p.Sender)
                   .WithMany()
                   .HasForeignKey(p => p.SenderId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.Messenger)
                   .WithMany()
                   .HasForeignKey(p => p.MessengerId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.Warehouseman)
                   .WithMany()
                   .HasForeignKey(p => p.WarehousemanId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
