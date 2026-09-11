using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Projekt.Infrastructure.Data;

namespace Infrastracture.Data.Seeding
{
    public static class AddressSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            if (await context.Addresses.AnyAsync())
                return;

            var addresses = new List<Address>
            {
                new() { Street = "Hlavní 1", City = "Praha", Zipcode = "11000" },
                new() { Street = "Hlavní 2", City = "Praha", Zipcode = "11000" },
                new() { Street = "Dlouhá 5", City = "Brno", Zipcode = "60200" },
                new() { Street = "Masarykova 10", City = "Brno", Zipcode = "60200" },
                new() { Street = "Nádražní 3", City = "Ostrava", Zipcode = "70200" },
                new() { Street = "Sokolská 15", City = "Plzeň", Zipcode = "30100" },
                new() { Street = "Komenského 8", City = "Olomouc", Zipcode = "77900" },
                new() { Street = "Palackého 20", City = "Pardubice", Zipcode = "53002" },
                new() { Street = "Jiráskova 12", City = "Hradec Králové", Zipcode = "50003" },
                new() { Street = "Školní 7", City = "Liberec", Zipcode = "46001" },

                new() { Street = "Květná 9", City = "Zlín", Zipcode = "76001" },
                new() { Street = "Polní 4", City = "Tábor", Zipcode = "39001" },
                new() { Street = "U Lesa 18", City = "Písek", Zipcode = "39701" },
                new() { Street = "Na Vyhlídce 6", City = "Karlovy Vary", Zipcode = "36001" },
                new() { Street = "Lipová 22", City = "Jihlava", Zipcode = "58601" },
                new() { Street = "Horská 11", City = "Trutnov", Zipcode = "54101" },
                new() { Street = "Zahradní 3", City = "Kladno", Zipcode = "27201" },
                new() { Street = "Revoluční 14", City = "Most", Zipcode = "43401" },
                new() { Street = "Tylova 17", City = "Opava", Zipcode = "74601" },
                new() { Street = "Lázeňská 25", City = "Františkovy Lázně", Zipcode = "35101" }
            };

            context.Addresses.AddRange(addresses);
            await context.SaveChangesAsync();
        }
    }
}
