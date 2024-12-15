using System.ComponentModel.DataAnnotations;

namespace frontend.Models
{
    public class StockViewModel
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Purchase { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long MarketCap { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();  
    }
} 