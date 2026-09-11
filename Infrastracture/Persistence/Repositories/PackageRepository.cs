using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Projekt.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Infrastructure.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly ApplicationDbContext _context;

        public PackageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Package package)
        {
            await _context.Packages.AddAsync(package);
        }

        public async Task<Package?> GetByIdAsync(int id)
        {
            return await _context.Packages
                .Include(p => p.PickupAddress)
                .Include(p => p.DeliveryAddress)
                .Include(p => p.StorageAddress)
                .Include(p => p.Sender)
                .Include(p => p.Messenger)
                .Include(p => p.Warehouseman)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Package>> GetAllAsync()
        {
            return await _context.Packages
                .Include(p => p.PickupAddress)
                .Include(p => p.DeliveryAddress)
                .Include(p => p.StorageAddress)
                .Include(p => p.Sender)
                .Include(p => p.Messenger)
                .Include(p => p.Warehouseman)
                .ToListAsync();
        }

        public async Task<List<Package>> GetBySenderAsync(int senderId)
        {
            return await _context.Packages
                .Include(p => p.PickupAddress)
                .Include(p => p.DeliveryAddress)
                .Include(p => p.StorageAddress)
                .Include(p => p.Sender)
                .Where(p => p.SenderId == senderId)
                .ToListAsync();
        }

        public async Task<List<Package>> GetByMessengerAndStatusAsync(int messengerId, string status)
        {
            return await _context.Packages
                .Include(p => p.PickupAddress)
                .Include(p => p.DeliveryAddress)
                .Include(p => p.StorageAddress)
                .Include(p => p.Storage)
                .Include(p => p.Messenger)
                .Where(p => p.MessengerId == messengerId && p.Status == status)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
