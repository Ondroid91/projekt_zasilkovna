using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Projekt.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class UserController : Controller
    {
        public IActionResult Admin()
        {
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("User_role");

            if (string.IsNullOrEmpty(username) || role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult Messenger()
        {
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("User_role");

            if (string.IsNullOrEmpty(username) || role != "Messenger")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult Warehouseman()
        {
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("User_role");

            if (string.IsNullOrEmpty(username) || role != "Warehouseman")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult Customer()
        {
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("User_role");

            if (string.IsNullOrEmpty(username) || role != "Customer")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}
