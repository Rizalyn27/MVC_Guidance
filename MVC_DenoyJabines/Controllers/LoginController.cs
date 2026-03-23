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
        // Database context
        public readonly AppDbContext _context;

        // Database context
        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // Login page
        public IActionResult Index()
        {
            return View();
        }

        // Registration page
        public IActionResult Register()
        {
            return View(new Users());
        }

        //Handle user registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Users user)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
                return View(user);

            // Check if username or email already exists
            if (await _context.User.AnyAsync(u => u.Username == user.Username || u.Email == user.Email))
            {
                ModelState.AddModelError("", "Username or Email already exists");
                return View(user);
            }

            // Hash the password before saving
            user.Password = HashPassword(user.Password);

            // Add and save the user to the database
            _context.Add(user);
            await _context.SaveChangesAsync();

            // Redirect to login page after successful registration
            return RedirectToAction("Login");
        }

        // Login page
        public IActionResult Login()
        {
            return View();
        }

        // user login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Username and Password are required";
                return View();
            }

            string hashedPassword = HashPassword(password);

            var user = await _context.User
                .FirstOrDefaultAsync(u => (u.Username == username || u.Email == username)
                                        && u.Password == hashedPassword);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            // CLAIM
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.UserId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // keeps user logged in
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Role base 
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

        //Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }

        // Hash Pass
        private string HashPassword(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToHexString(bytes);
            }
        }
    }
}
