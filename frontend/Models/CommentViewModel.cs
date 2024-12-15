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
        public string CreatedBy { get; set; } = string.Empty;
    }
} 