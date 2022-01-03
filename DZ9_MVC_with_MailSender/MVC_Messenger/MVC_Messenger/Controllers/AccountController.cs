using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVC_Messenger.Data;
using MVC_Messenger.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC_Messenger.Controllers
{
    public class AccountController : Controller
    {
        private readonly MessengerContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, MessengerContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation($"Login page visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"Login action {DateTime.Now.ToLongTimeString()} by '{model.Email}'");
                User user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
                _logger.LogInformation($"Login action fail {DateTime.Now.ToLongTimeString()} Email='{model.Email}' pass={model.Password}");
                ModelState.AddModelError("", "Incorrect email and/or password");
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation($"Register page visited at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"Register action with correct model at {DateTime.Now.ToLongTimeString()} " +
                    $" email='{model.Email}' Name={model.Name} PhoneNumber={model.PhoneNumber}");
                
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    // save user in dataBase
                    user = new User
                    {
                        Email = model.Email,
                        Password = model.Password,
                        Name = model.Name,
                        PhoneNumber = model.PhoneNumber,
                    };
                    Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                    if (userRole != null)
                        user.Role = userRole;

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogInformation($"Warning Register action incorrect model {DateTime.Now.ToLongTimeString()} " +
                        $" email='{model.Email}' Name={model.Name} PhoneNumber={model.PhoneNumber} {model.Password}'/'{model.ConfirmPassword}");
                    ModelState.AddModelError("", "Incorrect email and/or password");
                }                    
            }
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            _logger.LogInformation($"Authenticate action {DateTime.Now.ToLongTimeString()} " +
                    $" email='{user.Email}' Name={user.Name}");
            // create claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // create object ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // set cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation($"Logout at {DateTime.Now.ToLongTimeString()} by '{User.Identity.Name}'");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

