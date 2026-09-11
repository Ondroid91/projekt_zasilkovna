using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Projekt.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Infrastructure.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Warehouse>> GetWarehousesAsync()
            => await _context.Warehouses.Include(w => w.Storages).ToListAsync();

        public async Task<Warehouse?> GetWarehouseByIdAsync(int id)
            => await _context.Warehouses.Include(w => w.Storages).FirstOrDefaultAsync(w => w.Id == id);

        public async Task<Storage?> GetStorageIdAsync(int id)
            => await _context.Storages.Include(w => w.Warehouse).FirstOrDefaultAsync(w => w.Id == id);

        public async Task<Address?> GetAddressByIdAsync(int id)
            => await _context.Addresses.FindAsync(id);

        public IQueryable<Address> GetAddresses() => _context.Addresses.AsQueryable();


        public async Task<List<Package>> GetStoredPackagesAsync(int storageId)
            => await _context.Packages
                .Include(p => p.PickupAddress)
                .Include(p => p.DeliveryAddress)
                .Include(p => p.StorageAddress)
                .Where(p => p.Status == "stored" && p.WarehouseId == storageId)
                .ToListAsync();

        public async Task<List<Package>> GetPackagesToSendAsync(int storageId)
            => await _context.Packages
                .Include(p => p.PickupAddress)
                .Include(p => p.DeliveryAddress)
                .Include(p => p.Storage)
                .Where(p => p.Status == "stored" && p.WarehouseId == storageId)
                .ToListAsync();

        public async Task<List<Storage>> GetStoragesForWarehouseAsync(int warehouseId)
            => await _context.Storages
                .Where(s => s.WarehouseId == warehouseId && s.Type == "warehouse")
                .ToListAsync();

        public async Task<List<Storage>> GetStoragesForDepotAsync(int warehouseId)
            => await _context.Storages
                .Where(s => s.WarehouseId == warehouseId && s.Type == "depot")
                .ToListAsync();

        public async Task<Package?> GetPackageByIdAsync(int packageId)
            => await _context.Packages
                .Include(p => p.PickupAddress)
                .Include(p => p.DeliveryAddress)
                .Include(p => p.StorageAddress)
                .Include(p => p.Sender)
                .Include(p => p.Messenger)
                .Include(p => p.Warehouseman)
                .FirstOrDefaultAsync(p => p.Id == packageId);

        public async Task<Address?> GetAddressByDetailsAsync(string street, string city, string zipcode)
            => await _context.Addresses.FirstOrDefaultAsync(a =>
                a.Street == street && a.City == city && a.Zipcode == zipcode);

        public async Task<List<Address>> GetAllAddressesAsync()
        {
            return await _context.Addresses
                .OrderBy(a => a.City)
                .ThenBy(a => a.Street)
                .ToListAsync();
        }

        public async Task AddAsync(Address address) => await _context.Addresses.AddAsync(address);

        public async Task<(bool Success, string ErrorMessage)> CreateAddressAsync(string street, string city, string zipcode)
        {
            try
            {
                var existing = await GetAddressByDetailsAsync(street, city, zipcode);
                if (existing != null) return (false, "Adresa již existuje");

                await AddAsync(new Address { Street = street, City = city, Zipcode = zipcode });
                await _context.SaveChangesAsync();
                return (true, "");
            }
            catch (System.Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<Address?> GetByIdAsync(int id)
        {
            return await _context.Addresses.FindAsync(id);
        }


        public async Task<(bool Success, string ErrorMessage)> DeleteAddressAsync(int id)
        {
            try
            {
                var address = await GetAddressByIdAsync(id);
                if (address == null) return (false, "Adresa nenalezena");

                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
                return (true, "");
            }
            catch (System.Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
