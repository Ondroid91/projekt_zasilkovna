using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projekt.Infrastructure.Data;
using Projekt.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IUserRepository _userRepository;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);
            return await MapToDomainAsync(appUser);
        }


        public async Task<ApplicationUser?> GetUserEntityByIdAsync(int id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }


        public async Task<User?> GetByNameAsync(string username)
        {
            var appUser = await _userManager.FindByNameAsync(username);
            return await MapToDomainAsync(appUser);
        }

        public async Task<List<User>> GetAllAsync()
        {
            var appUsers = await _userManager.Users.ToListAsync();
            var users = new List<User>();
            foreach (var u in appUsers)
                users.Add(await MapToDomainAsync(u));
            return users;
        }

        public async Task<(bool Success, string ErrorMessage)> CreateUserAsync(
            string username,
            string email,
            string phone,
            string password,
            string role,
            int? workplaceId = null)
        {
            if (await UserExistsAsync(username, email))
                return (false, "Uživatel s tímto jménem nebo emailem již existuje.");

            var appUser = new ApplicationUser
            {
                UserName = username,
                Email = email,
                PhoneNumber = phone,
                WorkplaceId = workplaceId
            };

            var result = await _userManager.CreateAsync(appUser, password);
            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

            if (!string.IsNullOrEmpty(role))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole<int> { Name = role });

                await _userManager.AddToRoleAsync(appUser, role);
            }

            var domainUser = new Domain.Entities.User
            {
                Id = appUser.Id,
                Name = username,
                Email = email,
                Phone = phone,
                Role = role,
                WorkplaceId = workplaceId ?? 0
            };

            await _userRepository.AddAsync(domainUser);

            return (true, "");
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var roles = await _userRepository.GetRolesAsync(id);

            if (roles.Contains("Admin"))
                return false;

            var role = roles.FirstOrDefault();

            if (role != null)
                await _userRepository.HandleUserPackagesAsync(id, role);

            await _userRepository.RemoveDomainUserAsync(id);

            return await _userRepository.DeleteIdentityUserAsync(id);
        }


        public async Task<bool> UserExistsAsync(string name, string email)
        {
            var userByName = await _userManager.FindByNameAsync(name);
            var userByEmail = await _userManager.FindByEmailAsync(email);
            return userByName != null || userByEmail != null;
        }

        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return null;

            return await MapToDomainAsync(appUser);
        }
        private async Task<User> MapToDomainAsync(ApplicationUser u)
        {
            var roles = await _userManager.GetRolesAsync(u);

            return new User
            {
                Id = u.Id,
                Name = u.UserName!,
                Email = u.Email!,
                Phone = u.PhoneNumber!,
                Role = roles.FirstOrDefault() ?? "Customer",
                WorkplaceId = u.WorkplaceId ?? 0,
                EmployeeType = u.EmployeeType
            };
        }
    }
}
