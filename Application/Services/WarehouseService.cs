using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _repository;

        public WarehouseService(IWarehouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Warehouse>> GetWarehousesAsync() => await _repository.GetWarehousesAsync();

        public async Task<Warehouse?> GetWarehouseId(int id)
            => await _repository.GetWarehouseByIdAsync(id);
        public async Task<List<Package>> GetStoredPackagesAsync(int userWorkplace)
            => await _repository.GetStoredPackagesAsync(userWorkplace);

        public async Task<List<Package>> GetPackagesToSendAsync(int userWorkplace)
            => await _repository.GetPackagesToSendAsync(userWorkplace);

        public async Task<List<Storage>> GetStoragesForWarehouseAsync(int warehouseId)
            => await _repository.GetStoragesForWarehouseAsync(warehouseId);

        public async Task<List<Storage>> GetStoragesForDepotAsync(int warehouseId)
            => await _repository.GetStoragesForDepotAsync(warehouseId);

        public IQueryable<Address> GetAddresses() => _repository.GetAddresses();

        public async Task<(bool Success, string ErrorMessage)> AssignStorageAsync(int packageId, int storageId)
        {
            var package = await _repository.GetPackageByIdAsync(packageId);
            if (package == null) return (false, "Balíček nebyl nalezen.");

            package.StorageAddressId = storageId;
            package.Storage = await _repository.GetStorageIdAsync(storageId);
            package.Status = "stored";

            await _repository.SaveAsync();
            return (true, "");
        }

        public async Task<(bool Success, string ErrorMessage)> AssignStorageDepoAsync(int packageId, int storageId)
        {
            var package = await _repository.GetPackageByIdAsync(packageId);
            if (package == null) return (false, "Balíček nebyl nalezen.");

            package.StorageAddressId = storageId;
            package.Storage = await _repository.GetStorageIdAsync(storageId);
            package.Status = "in_delivery";

            await _repository.SaveAsync();
            return (true, "");
        }

        public async Task<(bool Success, string ErrorMessage)> StorePackageAsync(int packageId)
        {
            var package = await _repository.GetPackageByIdAsync(packageId);
            if (package == null) return (false, "Balíček nebyl nalezen.");

            package.Status = "stored";
            await _repository.SaveAsync();
            return (true, "");
        }

        public async Task<(bool Success, string ErrorMessage)> SendPackageAsync(int packageId)
        {
            var package = await _repository.GetPackageByIdAsync(packageId);
            if (package == null) return (false, "Balíček nebyl nalezen.");

            package.Status = "in_delivery";
            await _repository.SaveAsync();
            return (true, "");
        }

        public async Task<Address?> GetAddressByDetailsAsync(string street, string city, string zipcode)
            => await _repository.GetAddressByDetailsAsync(street, city, zipcode);

        public async Task<bool> DeleteAddressAsync(int id)
        {
            var result = await _repository.DeleteAddressAsync(id);
            return result.Success;
        }

        public async Task<Address?> GetAddressByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Address>> GetAllAddressesAsync()
        {
            return await _repository.GetAllAddressesAsync();
        }
        public async Task<(bool Success, string ErrorMessage)> CreateAddressAsync(string street, string city, string zipcode)
        {
            if (string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(zipcode))
                return (false, "Chybí adresa");

            return await _repository.CreateAddressAsync(street, city, zipcode);
        }

    }
}
