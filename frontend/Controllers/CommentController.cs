using Microsoft.AspNetCore.Mvc;
using frontend.Models;
using frontend.Services;

namespace frontend.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApiService _apiService;

        public CommentController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string symbol, string title, string content, int stockId)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                TempData["Error"] = "Title and content are required.";
                return RedirectToAction("Details", "Stock", new { id = stockId });
            }

            var (success, error) = await _apiService.AddComment(symbol, title, content);
            
            if (!success)
            {
                TempData["Error"] = error ?? "Failed to add comment.";
                return RedirectToAction("Details", "Stock", new { id = stockId });
            }

            TempData["Success"] = "Comment added successfully!";
            return RedirectToAction("Details", "Stock", new { id = stockId });
        }
    }
} 