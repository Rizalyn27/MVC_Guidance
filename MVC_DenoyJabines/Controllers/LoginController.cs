using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_DenoyJabines.Data;
using MVC_DenoyJabines.Models;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace MVC_DenoyJabines.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        // Inject database context
        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // STATIC ADMIN CREDENTIALS
        private const string StaticAdminUsername = "Admin";
        private const string StaticAdminPassword = "Admin#01";

        // LOGIN PAGE
        public IActionResult Login()
        {
            return View();
        }

        // LOGIN ACTION
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Username and Password are required";
                return View();
            }

            // STATIC ADMIN LOGIN
            if (username == StaticAdminUsername && password == StaticAdminPassword)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Admin"),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("UserId", "0") // Static admin ID
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { IsPersistent = true });

                return RedirectToAction("Admin", "Home");
            }

            // DATABASE LOGIN
            string hashedPassword = HashPassword(password);

            var user = await _context.User
                .FirstOrDefaultAsync(u =>
                    (u.Username == username || u.Email == username) // username or email login
                    && u.Password == hashedPassword);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Your account is inactive. Contact admin.");
                return View();
            }

            // SET CLAIMS
            var claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.UserId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claimsList, CookieAuthenticationDefaults.AuthenticationScheme);

            // SIGN IN USING COOKIE AUTHENTICATION
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true });

            // REDIRECT BASED ON ROLE
            switch (user.Role)
            {
                case "Admin":
                    return RedirectToAction("Admin", "Home");

                case "Counselor":
                    return RedirectToAction("Home", "Home");

                case "Student":
                    return RedirectToAction("StudentsHome", "Home");

                default:
                    return RedirectToAction("StudentsHome", "Home");
            }
        }

        // REGISTER PAGE
        public IActionResult Register()
        {
            return View(new Users());
        }

        // REGISTER ACTION
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Users user)
        {
            if (!ModelState.IsValid)
                return View(user);

            // Check if username or email already exists
            if (await _context.User.AnyAsync(u => u.Username == user.Username || u.Email == user.Email))
            {
                ModelState.AddModelError("", "Username or Email already exists");
                return View(user);
            }

            // HASH PASSWORD BEFORE SAVING
            user.Password = HashPassword(user.Password);

            _context.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // LOGOUT ACTION
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // HASH PASSWORD METHOD
        private string HashPassword(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToHexString(bytes); // Convert hash bytes to hex string
            }
        }
    }
}