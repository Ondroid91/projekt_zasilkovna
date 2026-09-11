using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Infrastructure.Data.Configuration;

namespace Projekt.Infrastructure.Data.Configuration.MySQL
{
    public class WarehouseConfiguration : WarehouseConfigurationBase
    {
        public override void Configure(EntityTypeBuilder<Domain.Entities.Warehouse> builder)
        {
            base.Configure(builder);

            builder.Property(w => w.Name)
                   .HasMaxLength(200)
                   .IsRequired();
        }
    }
}
