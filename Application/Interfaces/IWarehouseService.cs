using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWarehouseService
    {
        Task<List<Warehouse>> GetWarehousesAsync();
        Task<Warehouse?> GetWarehouseId(int id);
        Task<List<Package>> GetStoredPackagesAsync(int userWorkplace);
        Task<List<Package>> GetPackagesToSendAsync(int userWorkplace);
        Task<List<Storage>> GetStoragesForWarehouseAsync(int warehouseId);
        Task<List<Storage>> GetStoragesForDepotAsync(int warehouseId);
        IQueryable<Address> GetAddresses();

        Task<(bool Success, string ErrorMessage)> AssignStorageAsync(int packageId, int storageId);
        Task<(bool Success, string ErrorMessage)> AssignStorageDepoAsync(int packageId, int storageId);
        Task<(bool Success, string ErrorMessage)> StorePackageAsync(int packageId);
        Task<(bool Success, string ErrorMessage)> SendPackageAsync(int packageId);

        Task<Address?> GetAddressByDetailsAsync(string street, string city, string zipcode);
        Task<Address?> GetAddressByIdAsync(int id);
        Task<List<Address>> GetAllAddressesAsync();
        Task<bool> DeleteAddressAsync(int id);
        Task<(bool Success, string ErrorMessage)> CreateAddressAsync(string street, string city, string zipcode);
    }
}




