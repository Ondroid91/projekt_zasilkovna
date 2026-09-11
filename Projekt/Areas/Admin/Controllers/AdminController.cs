using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPackageService _packageService;
        private readonly IWarehouseService _warehouseService;

        public AdminController(
            IUserService userService,
            IPackageService packageService,
            IWarehouseService warehouseService)
        {
            _userService = userService;
            _packageService = packageService;
            _warehouseService = warehouseService;
        }

        public async Task<IActionResult> ManageUsers(string sortBy = "id")
        {
            var users = await _userService.GetAllAsync();
            var warehouses = await _warehouseService.GetWarehousesAsync();

            users = sortBy.ToLower() switch
            {
                "username" => users.OrderBy(u => u.Name).ToList(),
                "email" => users.OrderBy(u => u.Email).ToList(),
                "phone" => users.OrderBy(u => u.Phone).ToList(),
                "role" => users.OrderBy(u => u.Role).ToList(),
                _ => users.OrderBy(u => u.Id).ToList()
            };

            ViewBag.Warehouses = warehouses;

            return View(users);
        }

        public async Task<IActionResult> ManagePackage()
        {
            var packages = await _packageService.GetAllPackagesAsync();
            var users = await _userService.GetAllAsync();
            var warehouses = await _warehouseService.GetWarehousesAsync();

            ViewBag.Warehouses = warehouses;
            ViewBag.Messengers = users.Where(u => u.Role == "Messenger");
            ViewBag.HasPackages = packages.Any();

            return View(packages);
        }

        public IActionResult ManageAddress(string sortBy = "id")
        {
            var addresses = _warehouseService.GetAddresses();

            addresses = sortBy.ToLower() switch
            {
                "street" => addresses.OrderBy(a => a.Street),
                "city" => addresses.OrderBy(a => a.City),
                "zipcode" => addresses.OrderBy(a => a.Zipcode),
                _ => addresses.OrderBy(a => a.Id)
            };

            return View(addresses.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(ManageUsers));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            await _warehouseService.DeleteAddressAsync(id);
            return RedirectToAction(nameof(ManageAddress));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(
            string username, string email, string phone, string password, string role, int? warehouseId)
        {
            if (role == "Warehouseman" && !warehouseId.HasValue)
            {
                ViewBag.ErrorMessage = "Nelze vytvořit skladníka bez vybraného pracoviště.";
                ViewBag.Warehouses = await _warehouseService.GetWarehousesAsync();
                var users = await _userService.GetAllAsync();
                return View("ManageUsers", users);
            }

            var result = await _userService.CreateUserAsync(username, email, phone, password, role, warehouseId);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                ViewBag.Warehouses = await _warehouseService.GetWarehousesAsync();
                var users = await _userService.GetAllAsync();
                return View("ManageUsers", users);
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(string street, string city, string zipcode)
        {
            var result = await _warehouseService.CreateAddressAsync(street, city, zipcode);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                var addresses = _warehouseService.GetAddresses().ToList();
                return View("ManageAddress", addresses);
            }

            return RedirectToAction(nameof(ManageAddress));
        }

        [HttpPost]
        public async Task<IActionResult> AssignMessenger(int packageId, int messengerId)
        {
            var result = await _packageService.AssignMessengerAsync(packageId, messengerId);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View("ViewPackage");
            }

            return RedirectToAction(nameof(ManagePackage));
        }

        [HttpPost]
        public async Task<IActionResult> AssignWarehouse(int packageId, int warehouseId)
        {
            var result = await _packageService.AssignWarehouseAsync(packageId, warehouseId);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View("ViewPackage");
            }

            return RedirectToAction(nameof(ManagePackage));
        }

        [HttpPost]
        public async Task<IActionResult> CancelPackage(int packageId)
        {
            var result = await _packageService.CancelPackageAsync(packageId);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return RedirectToAction(nameof(ManagePackage));
            }

            return RedirectToAction(nameof(ManagePackage));
        }
    }
}
