using frontend.Models;
using frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace frontend.Controllers
{
    public class StockController : BaseController
    {
        private readonly ApiService _apiService;

        public StockController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var (success, stocks, error) = await _apiService.GetAllStocks();
            
            if (!success)
            {
                TempData["Error"] = error;
                return View(new List<StockViewModel>());
            }

            return View(stocks);
        }

        public async Task<IActionResult> Details(int id)
        {
            var (success, stock, error) = await _apiService.GetStockById(id);
            
            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction(nameof(Index));
            }

            return View(stock);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int stockId, string title, string content)
        {
            var (success, error) = await _apiService.AddComment(stockId.ToString(), title, content);
            
            if (!success)
            {
                TempData["Error"] = error;
            }
            else
            {
                TempData["Success"] = "Comment added successfully.";
            }

            return RedirectToAction(nameof(Details), new { id = stockId });
        }
    }
} 