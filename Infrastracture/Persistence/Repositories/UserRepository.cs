using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projekt.Infrastructure.Data;
using Projekt.Infrastructure.Identity;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        var appUser = await _userManager.FindByIdAsync(id);
        return await MapToDomainAsync(appUser);
    }

    public async Task<User?> GetByNameAsync(string username)
    {
        var appUser = await _userManager.FindByNameAsync(username);
        return await MapToDomainAsync(appUser);
    }

    public IQueryable<User> GetAll()
    {
        return _userManager.Users.Select(u => new User
        {
            Name = u.UserName!,
            Email = u.Email!,
            EmployeeType = u.EmployeeType,
            WorkplaceId = u.WorkplaceId ?? 0,
            Role = ""
        }).AsQueryable();
    }

    public async Task AddAsync(User user, string password, string roleName)
    {
        var appUser = new ApplicationUser
        {
            UserName = user.Name,
            Email = user.Email,
            WorkplaceId = user.WorkplaceId,
            EmployeeType = user.EmployeeType
        };

        var result = await _userManager.CreateAsync(appUser, password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
        }


        await _userManager.AddToRoleAsync(appUser, roleName);
    }

    public async Task RemoveAsync(User user)
    {
        var appUser = await _userManager.FindByNameAsync(user.Name);
        if (appUser != null)
            await _userManager.DeleteAsync(appUser);
    }

    public async Task<bool> UserExistsAsync(string name, string email)
    {
        return await _userManager.Users
            .AnyAsync(u => u.UserName == name || u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        _context.DomainUsers.Add(user);
        await _context.SaveChangesAsync();
    }

    private async Task<User?> MapToDomainAsync(ApplicationUser? appUser)
    {
        if (appUser == null) return null;

        var roles = await _userManager.GetRolesAsync(appUser);
        return new User
        {
            Name = appUser.UserName!,
            Email = appUser.Email!,
            EmployeeType = appUser.EmployeeType,
            WorkplaceId = appUser.WorkplaceId ?? 0,
            Role = roles.FirstOrDefault() ?? "Customer"
        };
    }


    public async Task<IList<string>> GetRolesAsync(int userId)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());
        if (appUser == null) return new List<string>();

        return await _userManager.GetRolesAsync(appUser);
    }

    public async Task<User?> GetDomainUserByIdAsync(int id)
    {
        return await _context.DomainUsers.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task HandleUserPackagesAsync(int userId, string role)
    {
        var packages = await _context.Packages
            .Where(p =>
                p.SenderId == userId ||
                p.MessengerId == userId ||
                p.WarehousemanId == userId)
            .ToListAsync();

        switch (role)
        {
            case "Customer":
                _context.Packages.RemoveRange(
                    packages.Where(p => p.SenderId == userId)
                );
                break;

            case "Messenger":
                foreach (var p in packages.Where(p => p.MessengerId == userId))
                    p.MessengerId = null;
                break;

            case "Warehouseman":
                foreach (var p in packages.Where(p => p.WarehousemanId == userId))
                    p.WarehousemanId = null;
                break;
        }

        await _context.SaveChangesAsync();
    }
    public async Task RemoveDomainUserAsync(int id)
    {
        var domainUser = await _context.DomainUsers.FirstOrDefaultAsync(u => u.Id == id);
        if (domainUser != null)
        {
            _context.DomainUsers.Remove(domainUser);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> DeleteIdentityUserAsync(int id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString());
        if (appUser == null) return true;

        var result = await _userManager.DeleteAsync(appUser);
        return result.Succeeded;
    }
}
