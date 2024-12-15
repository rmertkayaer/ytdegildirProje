using System.ComponentModel.DataAnnotations;

namespace frontend.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? StockId { get; set; }
        public string AppUserId { get; set; } = string.Empty;
        public AppUserViewModel AppUser { get; set; } = new AppUserViewModel();

        // Helper property to display username
        public string CreatedBy => AppUser?.UserName ?? "Anonymous";
    }
} 