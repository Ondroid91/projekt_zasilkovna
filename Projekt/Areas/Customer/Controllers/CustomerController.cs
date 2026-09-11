using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Areas.Customer.Controllers
{
    [Authorize(Roles = "Customer")]
    [Area("Customer")]
    public class CustomerController : Controller
    {
        private readonly IPackageService _packageService;
        private readonly IWarehouseService _warehouseService;

        public CustomerController(IPackageService packageService, IWarehouseService warehouseService)
        {
            _packageService = packageService;
            _warehouseService = warehouseService;
        }

        public IActionResult CreatePackage()
        {
            var addresses = _warehouseService.GetAddresses().ToList();
            return View(addresses);
        }

        public async Task<IActionResult> ViewPackage()
        {
            var senderId = HttpContext.Session.GetInt32("UserId");
            if (!senderId.HasValue)
                return RedirectToAction("Login", "Account");

            var packages = await _packageService.GetPackagesBySenderAsync(senderId.Value);

            ViewBag.HasPackages = packages.Any();

            return View(packages);
        }


        [HttpPost]
        public async Task<IActionResult> OnCreatePackage(
            string name,
            decimal weight,
            int pickupAddressId,
            int deliveryAddressId)
        {
            var senderId = HttpContext.Session.GetInt32("UserId");

            if (!senderId.HasValue)
            {
                ViewBag.ErrorMessage = "Uživatel není přihlášen!";
                var addresses = await _warehouseService.GetAllAddressesAsync();
                return View("CreatePackage", addresses);
            }


            var result = await _packageService.CreatePackageAsync(
                senderId.Value,
                name,
                weight,
                pickupAddressId,
                deliveryAddressId
            );

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                var addresses = await _warehouseService.GetAllAddressesAsync();
                return View("CreatePackage", addresses);
            }

            return RedirectToAction("ViewPackage");
        }


        [HttpPost]
        public async Task<IActionResult> MyPackages()
        {
            var senderId = HttpContext.Session.GetInt32("UserId");

            if (!senderId.HasValue)
                return RedirectToAction("Login", "Account");

            var packages = await _packageService.MyPackagesAsync(senderId.Value);

            ViewBag.HasPackages = packages.Any();
            return View(packages);
        }
    }
}