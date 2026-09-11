using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projekt.Infrastructure.Identity;
using Projekt.Infrastructure.Identity.Interfaces;
using System.Threading.Tasks;
using Projekt.Models;


namespace Projekt.Areas.Account.Controllers
{
    [Area("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Neplatné přihlašovací údaje");
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            var roles = await _userManager.GetRolesAsync(user);

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.UserName);
            HttpContext.Session.SetString("User_role", roles.FirstOrDefault() ?? "Customer");
            if (user.WorkplaceId.HasValue)
            {
                HttpContext.Session.SetInt32("User_workplace", user.WorkplaceId.Value);
            }

            var action = roles.FirstOrDefault() switch
            {
                "Admin" => "Admin",
                "Customer" => "Customer",
                "Messenger" => "Messenger",
                "Warehouseman" => "Warehouseman",
                _ => "Customer"
            };

            return RedirectToAction(action, "User", new { Area = "User" });
        }
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            (bool success, string errorMessage) = await _userService.CreateUserAsync(
                username: model.Username,
                email: model.Email,
                phone: model.Phone,
                password: model.Password,
                role: model.Role,
                null
            );

            if (!success)
            {
                ModelState.AddModelError("", errorMessage);
                return View(model);
            }

            var appUser = await _userManager.FindByNameAsync(model.Username);
            await _signInManager.SignInAsync(appUser, false);

            HttpContext.Session.SetInt32("UserId", appUser.Id);
            HttpContext.Session.SetString("Username", appUser.UserName);
            HttpContext.Session.SetString("User_role", model.Role);

            return RedirectToAction("Customer", "User", new { Area = "User" });
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}