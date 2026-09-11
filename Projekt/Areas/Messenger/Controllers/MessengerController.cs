using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Areas.Messenger.Controllers
{
    [Authorize(Roles = "Messenger")]
    [Area("Messenger")]
    public class MessengerController : Controller
    {
        private readonly IPackageService _packageService;

        public MessengerController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public async Task<IActionResult> DeliverPackage()
        {
            var messengerId = HttpContext.Session.GetInt32("UserId");
            if (!messengerId.HasValue)
                return RedirectToAction("Login", "Account");

            var packages = await _packageService.GetPackagesInDeliveryAsync(messengerId.Value);
            ViewBag.HasPackages = packages.Any();
            return View(packages);
        }

        public async Task<IActionResult> PickupPackage()
        {
            var messengerId = HttpContext.Session.GetInt32("UserId");
            if (!messengerId.HasValue)
                return RedirectToAction("Login", "Account");

            var packages = await _packageService.GetPackagesToPickupAsync(messengerId.Value);
            ViewBag.HasPackages = packages.Any();
            return View(packages);
        }

        [HttpPost]
        public async Task<IActionResult> OnPickup(int packageId)
        {
            var result = await _packageService.PickupPackageAsync(packageId);
            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View("PickupPackage");
            }

            return RedirectToAction("PickupPackage");
        }

        [HttpPost]
        public async Task<IActionResult> OnDeliver(int packageId)
        {
            var result = await _packageService.DeliverPackageAsync(packageId);
            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View("DeliverPackage");
            }

            return RedirectToAction("DeliverPackage");
        }
    }
}
