using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Projekt.Infrastructure.Data;
using Domain.Entities;

namespace Infrastracture.Data.Seeding
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = services.GetRequiredService<ApplicationDbContext>();

            string[] roles = { "Admin", "Customer", "Messenger", "Warehouseman" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
                }
            }

            // ---- ADMIN ----
            var adminEmail = "admin@zasilkovna.cz";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "+420777000001",
                    PhoneNumberConfirmed = true,
                    EmployeeType = "Admin"
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");

                dbContext.DomainUsers.Add(new User
                {
                    Name = "Admin",
                    Email = adminEmail,
                    Phone = "+420777000001",
                    Role = "Admin",
                    EmployeeType = "Admin"
                });
            }

            // ---- CUSTOMER ----
            var customerEmail = "customer@zasilkovna.cz";
            var customer = await userManager.FindByEmailAsync(customerEmail);
            if (customer == null)
            {
                customer = new ApplicationUser
                {
                    UserName = customerEmail,
                    Email = customerEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "+420777000002",
                    PhoneNumberConfirmed = true,
                    EmployeeType = "Customer"
                };
                await userManager.CreateAsync(customer, "Customer123!");
                await userManager.AddToRoleAsync(customer, "Customer");

                dbContext.DomainUsers.Add(new User
                {
                    Name = "Customer",
                    Email = customerEmail,
                    Phone = "+420777000002",
                    Role = "Customer",
                    EmployeeType = "Customer"
                });
            }

            // ---- MESSENGER ----
            var messengerEmail = "messenger@zasilkovna.cz";
            var messenger = await userManager.FindByEmailAsync(messengerEmail);
            if (messenger == null)
            {
                messenger = new ApplicationUser
                {
                    UserName = messengerEmail,
                    Email = messengerEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "+420777000003",
                    PhoneNumberConfirmed = true,
                    EmployeeType = "Messenger"
                };
                await userManager.CreateAsync(messenger, "Messenger123!");
                await userManager.AddToRoleAsync(messenger, "Messenger");

                dbContext.DomainUsers.Add(new User
                {
                    Name = "Messenger",
                    Email = messengerEmail,
                    Phone = "+420777000003",
                    Role = "Messenger",
                    EmployeeType = "Messenger"
                });
            }

            // ---- WAREHOUSEMAN ----
            var warehousemanEmail = "warehouse@zasilkovna.cz";
            var warehouseman = await userManager.FindByEmailAsync(warehousemanEmail);
            if (warehouseman == null)
            {
                warehouseman = new ApplicationUser
                {
                    UserName = warehousemanEmail,
                    Email = warehousemanEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "+420777000004",
                    PhoneNumberConfirmed = true,
                    EmployeeType = "Warehouseman",
                    WorkplaceId = 1
                };
                await userManager.CreateAsync(warehouseman, "Warehouse123!");
                await userManager.AddToRoleAsync(warehouseman, "Warehouseman");

                dbContext.DomainUsers.Add(new User
                {
                    Name = "Warehouseman",
                    Email = warehousemanEmail,
                    Phone = "+420777000004",
                    Role = "Warehouseman",
                    EmployeeType = "Warehouseman",
                    WorkplaceId = 1
                });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
