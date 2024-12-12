using frontend.Models;
using frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace frontend.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;
        private readonly IConfiguration _configuration;

        public AccountController(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("JWTToken") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("JWTToken") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var (success, user, error) = await _apiService.Register(model);
                if (success && user != null)
                {
                    // Automatically log in the user after successful registration
                    HttpContext.Session.SetString("JWTToken", user.Token);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("UserEmail", user.Email);

                    // Set authentication cookie
                    Response.Cookies.Append("X-Access-Token", user.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });

                    TempData["Success"] = "Registration successful! Welcome to Stock App.";
                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError("", error ?? "Registration failed. Please try again.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var (success, user, error) = await _apiService.Login(model);
                if (success && user != null)
                {
                    // Store the token and user info in session
                    HttpContext.Session.SetString("JWTToken", user.Token);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("UserEmail", user.Email);

                    // Set authentication cookie
                    Response.Cookies.Append("X-Access-Token", user.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7) // Token expiration
                    });

                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError("", error ?? "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();
            
            // Clear authentication cookie
            Response.Cookies.Delete("X-Access-Token");

            return RedirectToAction("Login");
        }

        // Helper method to check if user is authenticated
        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetString("JWTToken") != null;
        }
    }
} 