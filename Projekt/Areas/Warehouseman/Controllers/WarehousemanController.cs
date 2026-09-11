using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Areas.Warehouseman.Controllers
{
    [Authorize(Roles = "Warehouseman")]
    [Area("Warehouseman")]
    public class WarehousemanController : Controller
    {
        private readonly IWarehouseService _warehouseService;

        public WarehousemanController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        public async Task<IActionResult> StorePackage()
        {
            var workplaceId = HttpContext.Session.GetInt32("User_workplace");
            if (!workplaceId.HasValue)
                return RedirectToAction("Login", "Account");

            var packages = await _warehouseService
                .GetStoredPackagesAsync(workplaceId.Value);

            var storages = await _warehouseService
                .GetStoragesForWarehouseAsync(workplaceId.Value);

            ViewBag.Storages = storages;
            ViewBag.HasPackages = packages.Any();

            return View(packages);
        }

        public async Task<IActionResult> SendPackage()
        {
            var userWorkplace = HttpContext.Session.GetInt32("User_workplace");
            if (!userWorkplace.HasValue)
                return RedirectToAction("Login", "Account");

            var packages = await _warehouseService.GetPackagesToSendAsync(userWorkplace.Value);
            var storages = await _warehouseService.GetStoragesForDepotAsync(userWorkplace.Value);

            ViewBag.Storages = storages;
            ViewBag.HasPackages = packages.Any();
            return View(packages);
        }

        [HttpPost]
        public async Task<IActionResult> AssignStorage(int packageId, int storageId)
        {
            var result = await _warehouseService.AssignStorageAsync(packageId, storageId);
            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View("StorePackage");
            }
            return RedirectToAction("StorePackage");
        }

        [HttpPost]
        public async Task<IActionResult> AssignStorageDepo(int packageId, int storageId)
        {
            var result = await _warehouseService.AssignStorageDepoAsync(packageId, storageId);
            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View("SendPackage");
            }
            return RedirectToAction("SendPackage");
        }

        [HttpPost]
        public async Task<IActionResult> OnStore(int packageId)
        {
            var result = await _warehouseService.StorePackageAsync(packageId);
            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View("StorePackage");
            }
            return RedirectToAction("StorePackage");
        }

        [HttpPost]
        public async Task<IActionResult> OnSend(int packageId)
        {
            var result = await _warehouseService.SendPackageAsync(packageId);
            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View("SendPackage");
            }
            return RedirectToAction("SendPackage");
        }
    }
}
