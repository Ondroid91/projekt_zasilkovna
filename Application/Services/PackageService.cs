using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IWarehouseService _warehouseService;
        private readonly IUserService _userService;

        public PackageService(IPackageRepository packageRepository, IWarehouseService warehouseService, IUserService userService)
        {
            _packageRepository = packageRepository;
            _warehouseService = warehouseService;
            _userService = userService;
;
        }

        public async Task<List<Package>> GetAllPackagesAsync()
        {
            return await _packageRepository.GetAllAsync();
        }

        public async Task<List<Package>> GetPackagesBySenderAsync(int senderId)
        {
            return await _packageRepository.GetBySenderAsync(senderId);
        }

        public async Task<List<Package>> MyPackagesAsync(int senderId)
        {
            return await _packageRepository.GetBySenderAsync(senderId);
        }

        public async Task<List<Package>> GetPackagesInDeliveryAsync(int messengerId)
        {
            return await _packageRepository.GetByMessengerAndStatusAsync(messengerId, "in_delivery");//in_delivery
        }

        public async Task<List<Package>> GetPackagesToPickupAsync(int messengerId)
        {
            return await _packageRepository.GetByMessengerAndStatusAsync(messengerId, "to_pickup");
        }

        public async Task<(bool Success, string ErrorMessage)> AssignMessengerAsync(int packageId, int messengerId)
        {
            var package = await _packageRepository.GetByIdAsync(packageId);
            if (package == null) return (false, "Balíček nebyl nalezen");

            package.MessengerId = messengerId;

            if (package.MessengerId != null && package.StorageAddressId != null)
                package.Status = "to_pickup";

            return await SavePackageAsync(package);
        }

        public async Task<(bool Success, string ErrorMessage)> AssignWarehouseAsync(int packageId, int warehouseId)
        {
            var package = await _packageRepository.GetByIdAsync(packageId);
            if (package == null) return (false, "Balíček nebyl nalezen");

            package.StorageAddressId = warehouseId;
            var warehouse = await _warehouseService.GetWarehouseId(warehouseId);
            package.Warehouse = warehouse;


            if (package.MessengerId != null && package.StorageAddressId != null )
                package.Status = "to_pickup";

            return await SavePackageAsync(package);
        }

        public async Task<(bool Success, string ErrorMessage)> CancelPackageAsync(int packageId)
        {
            var package = await _packageRepository.GetByIdAsync(packageId);
            if (package == null) return (false, "Balíček nebyl nalezen");

            package.Status = "canceled";

            return await SavePackageAsync(package);
        }

        public async Task<(bool Success, string ErrorMessage)> PickupPackageAsync(int packageId)
        {
            var package = await _packageRepository.GetByIdAsync(packageId);
            if (package == null) return (false, "Balíček nebyl nalezen");

            package.Status = "stored";

            return await SavePackageAsync(package);
        }

        public async Task<(bool Success, string ErrorMessage)> DeliverPackageAsync(int packageId)
        {
            var package = await _packageRepository.GetByIdAsync(packageId);
            if (package == null) return (false, "Balíček nebyl nalezen");

            package.Status = "delivered";

            return await SavePackageAsync(package);
        }

        public async Task<(bool Success, string ErrorMessage)> CreatePackageAsync(
            int senderId,
            string name,
            decimal weight,
            int pickupAddressId,
            int deliveryAddressId)
        {
            var pickupAddress = await _warehouseService.GetAddressByIdAsync(pickupAddressId);
            var deliveryAddress = await _warehouseService.GetAddressByIdAsync(deliveryAddressId);
            var sender = await _userService.GetUserEntityByIdAsync(senderId);

            if (sender == null) return (false, "Odesílatel neexistuje v databázi");
            if (pickupAddress == null) return (false, "Odesílací adresa neexistuje");
            if (deliveryAddress == null) return (false, "Doručovací adresa neexistuje");

            var package = new Package
            {
                Name = name,
                Weight = weight,
                Status = "created",
                PickupAddressId = pickupAddress.Id,
                DeliveryAddressId = deliveryAddress.Id,
                SenderId = sender.Id,
            };

            try
            {
                await _packageRepository.AddAsync(package);
                await _packageRepository.SaveAsync();
                return (true, "");
            }
            catch (Exception ex)
            {
                string fullMessage = ex.Message;
                var inner = ex.InnerException;
                while (inner != null)
                {
                    fullMessage += " | Inner: " + inner.Message;
                    inner = inner.InnerException;
                }

                return (false, $"Chyba při ukládání balíčku: {fullMessage}");
            }
        }

        private async Task<(bool Success, string ErrorMessage)> SavePackageAsync(Package package)
        {
            try
            {
                await _packageRepository.SaveAsync();
                return (true, "");
            }
            catch (System.Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
