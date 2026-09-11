using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Infrastructure.Data.Configuration;
using Projekt.Infrastructure.Identity;

namespace Projekt.Infrastructure.Data.Configuration.MySQL
{
    public class UserConfiguration : UserConfigurationBase
    {
        public override void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            base.Configure(builder);


            builder.Property(u => u.EmployeeType)
                   .HasMaxLength(20)
                   .IsRequired(false);
        }
    }
}
