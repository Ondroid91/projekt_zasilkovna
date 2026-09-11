using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(string id);
        Task<ApplicationUser?> GetUserEntityByIdAsync(int id);
        Task<User?> GetByNameAsync(string username);
        Task<List<User>> GetAllAsync();
        Task<(bool Success, string ErrorMessage)> CreateUserAsync(string username, string email, string phone, string password, string role, int? warehouseId);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(string name, string email);
        Task<User?> ValidateUserAsync(string username, string password);
    }
}

