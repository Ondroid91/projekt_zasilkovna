using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Projekt.Infrastructure.Data;

namespace Infrastructure.Data.Seeding
{
    public static class WarehouseSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            if (await context.Warehouses.AnyAsync())
                return;

            var addresses = await context.Addresses.Take(3).ToListAsync();

            if (addresses.Count < 3)
                throw new Exception("Nedostatek adres pro Warehouse seeding.");

            var warehouses = new List<Warehouse>
            {
                new Warehouse
                {
                    Name = "Warehouse 1",
                    AddressId = addresses[0].Id,
                    Storages =
                    {
                        new Storage { Name = "A1" },
                        new Storage { Name = "A2" },
                        new Storage { Name = "A3" },
                        new Storage { Name = "Depot A1" , Type = "depot"},
                        new Storage { Name = "Depot A2" , Type = "depot"}
                    }
                },
                new Warehouse
                {
                    Name = "Warehouse 2",
                    AddressId = addresses[1].Id,
                    Storages =
                    {
                        new Storage { Name = "B1" },
                        new Storage { Name = "B2" },
                        new Storage { Name = "Depot B1" , Type = "depot"},
                        new Storage { Name = "Depot B2" , Type = "depot"}
                    }
                },
                new Warehouse
                {
                    Name = "Warehouse 3",
                    AddressId = addresses[2].Id,
                    Storages =
                    {
                        new Storage { Name = "C1" },
                        new Storage { Name = "C2" },
                        new Storage { Name = "C3" },
                        new Storage { Name = "C4" },
                        new Storage { Name = "Depot C1" , Type = "depot"},
                        new Storage { Name = "Depot C2" , Type = "depot"}
                    }
                }
            };

            context.Warehouses.AddRange(warehouses);
            await context.SaveChangesAsync();
        }
    }
}
