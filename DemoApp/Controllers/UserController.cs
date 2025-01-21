using DemoApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DemoApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserRegisters
                {
                    Email = model.Email,
                    Password = model.Password,
                    UserName = model.UserName,
                    CreatedAt = DateTime.Now
                };

                _context.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(model);
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Clear any existing authentication cookies
                await HttpContext.SignOutAsync();

                var user = await _context.UserRegister
                    .FirstOrDefaultAsync(u => u.Email == model.Email);
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == model.Email);

                if (user != null && model.Password == user.Password) // Verify password
                {
                    // Create claims for the authenticated user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // Store UserId
                        new Claim(ClaimTypes.Email, user.Email),                     // Store Email
                        new Claim(ClaimTypes.Name, user.UserName)                    // Store UserName
                    };

                    // Create a claims identity
                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    // Create a claims principal
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Sign in the user
                    await HttpContext.SignInAsync(claimsPrincipal);

                    if (student != null)
                    {
                        return RedirectToAction("Index", "Students");
                    }

                    // Logic to set session or cookie for login
                    return RedirectToAction("Add", "Students");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // Clears the authentication cookie
            Response.Cookies.Delete(".AspNetCore.Cookies"); // Delete the authentication cookie
            return RedirectToAction("Login", "User"); // Redirect to the login page
        }

        [Authorize]
        public IActionResult Profile()
        {
            // Get the logged-in user's UserId from claims
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (loggedInUserId == null)
            {
                return RedirectToAction("Login", "User");
            }

            // Fetch the logged-in user's details
            //var user = _context.UserRegister.FirstOrDefault(u => u.UserId.ToString() == loggedInUserId);

            //if (user == null)
            //{
            //    return NotFound();
            //}

            return RedirectToAction("ProfileView", "Students", new { id = loggedInUserId});
        }


        [HttpGet]
        public JsonResult CheckEmailUnique(string email)
        {
            bool isUnique = !_context.UserRegister.Any(u => u.Email.ToLower() == email.ToLower());
            return Json(isUnique);
        }
    }
}
