using Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByNameAsync(string username);
    IQueryable<User> GetAll();
    Task AddAsync(User user, string password, string roleName);
    Task RemoveAsync(User user);
    Task<bool> UserExistsAsync(string name, string email);
    Task AddAsync(User user);
    Task<IList<string>> GetRolesAsync(int userId);
    Task<User?> GetDomainUserByIdAsync(int id);
    Task HandleUserPackagesAsync(int userId, string role);
    Task RemoveDomainUserAsync(int id);
    Task<bool> DeleteIdentityUserAsync(int id);
}

