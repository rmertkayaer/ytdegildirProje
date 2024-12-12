using frontend.Models;
using frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace frontend.Controllers
{
    public class PortfolioController : BaseController
    {
        private readonly ApiService _apiService;

        public PortfolioController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var (success, stocks, error) = await _apiService.GetUserPortfolio();
            
            if (!success)
            {
                TempData["Error"] = error;
                return View(new List<PortfolioListViewModel>());
            }

            return View(stocks);
        }

        [HttpPost]
        public async Task<IActionResult> AddStock(string symbol)
        {
            var (success, error) = await _apiService.AddToPortfolio(symbol);
            
            if (!success)
            {
                TempData["Error"] = error;
            }
            else
            {
                TempData["Success"] = $"{symbol} has been added to your portfolio.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveStock(string symbol)
        {
            var (success, error) = await _apiService.RemoveFromPortfolio(symbol);
            
            if (!success)
            {
                TempData["Error"] = error;
            }
            else
            {
                TempData["Success"] = $"{symbol} has been removed from your portfolio.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 