using Microsoft.AspNetCore.Mvc;

namespace frontend.Controllers
{
    public class BilgiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 