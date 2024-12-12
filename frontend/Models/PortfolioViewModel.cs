namespace frontend.Models
{
    public class PortfolioViewModel
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }
        public string Industry { get; set; }
        public decimal MarketCap { get; set; }
    }
} 