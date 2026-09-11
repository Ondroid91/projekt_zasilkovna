using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPackageRepository
    {
        Task AddAsync(Package package);
        Task<Package?> GetByIdAsync(int id);
        Task<List<Package>> GetAllAsync();
        Task<List<Package>> GetBySenderAsync(int senderId);
        Task<List<Package>> GetByMessengerAndStatusAsync(int messengerId, string status);
        Task SaveAsync();
    }
}

