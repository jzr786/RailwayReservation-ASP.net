using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RailwayReservation.Models;
using RailwayReservation.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace RailwayReservation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
        }

        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if (!user.IsActive)
                {
                    ModelState.AddModelError("", "Your account is inactive. Please contact the administrator.");
                    return View(model);
                }

                
                user.LastLoginDate = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                await _signInManager.SignInAsync(user, model.RememberMe);
                return RedirectToAction("Index", "Home"); 
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        
        [HttpGet]
        [AllowAnonymous] 
        public IActionResult Register()
        {
            if (!User.IsInRole("Admin"))
            {
                ViewBag.Roles = new SelectList(new List<string> { "Customer" }); 
            }
            else
            {
                ViewBag.Roles = new SelectList(new List<string> { "Admin", "Customer" }); 
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            bool isAdmin = User.Identity.IsAuthenticated && User.IsInRole("Admin");

            
            if (string.IsNullOrEmpty(model.Role))
            {
                model.Role = "Customer"; 
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = isAdmin
                    ? new SelectList(new List<string> { "Admin", "Customer" })
                    : new SelectList(new List<string> { "Customer" });

                return View(model);
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("", "Email is already taken.");
                return View(model);
            }

            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                ModelState.AddModelError("", "Username is already taken.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                IsActive = true,
                RoleName = model.Role 
            };

            string password = string.IsNullOrWhiteSpace(model.Password)
                ? ApplicationUser.DefaultPassword
                : model.Password;

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Role));
                }

                await _userManager.AddToRoleAsync(user, model.Role);

                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewBag.Roles = isAdmin
                ? new SelectList(new List<string> { "Admin", "Customer" })
                : new SelectList(new List<string> { "Customer" });

            return View(model);
        }




        
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}




