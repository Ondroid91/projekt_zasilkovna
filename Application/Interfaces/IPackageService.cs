using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPackageService
    {
        Task<List<Package>> GetAllPackagesAsync();
        Task<List<Package>> GetPackagesBySenderAsync(int senderId);
        Task<List<Package>> MyPackagesAsync(int senderId);
        Task<List<Package>> GetPackagesInDeliveryAsync(int messengerId);
        Task<List<Package>> GetPackagesToPickupAsync(int messengerId);

        Task<(bool Success, string ErrorMessage)> AssignMessengerAsync(int packageId, int messengerId);
        Task<(bool Success, string ErrorMessage)> AssignWarehouseAsync(int packageId, int warehouseId);
        Task<(bool Success, string ErrorMessage)> CancelPackageAsync(int packageId);
        Task<(bool Success, string ErrorMessage)> PickupPackageAsync(int packageId);
        Task<(bool Success, string ErrorMessage)> DeliverPackageAsync(int packageId);

        Task<(bool Success, string ErrorMessage)> CreatePackageAsync(
             int senderId,
             string name,
             decimal weight,
             int pickupAddressId,
             int deliveryAddressId);
    }
}
