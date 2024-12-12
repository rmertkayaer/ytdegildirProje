using Microsoft.AspNetCore.Mvc;
using frontend.Services;

namespace frontend.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            // Only authenticated users can access this page
            return View();
        }
    }
}
