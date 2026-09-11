using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IWarehouseRepository
    {
        Task<List<Warehouse>> GetWarehousesAsync();
        Task<Warehouse?> GetWarehouseByIdAsync(int id);
        Task<Storage?> GetStorageIdAsync(int id);
        Task<Address?> GetAddressByIdAsync(int id);
        IQueryable<Address> GetAddresses();
        Task<List<Package>> GetStoredPackagesAsync(int storageId);
        Task<List<Package>> GetPackagesToSendAsync(int storageId);
        Task<List<Storage>> GetStoragesForWarehouseAsync(int warehouseId);
        Task<List<Storage>> GetStoragesForDepotAsync(int warehouseId);
        Task<Package?> GetPackageByIdAsync(int packageId);
        Task<Address?> GetAddressByDetailsAsync(string street, string city, string zipcode);
        Task<List<Address>> GetAllAddressesAsync();
        Task AddAsync(Address address);
        Task SaveAsync();
        Task<(bool Success, string ErrorMessage)> CreateAddressAsync(string street, string city, string zipcode);
        Task<Address?> GetByIdAsync(int id);
        Task<(bool Success, string ErrorMessage)> DeleteAddressAsync(int id);
    }
}




