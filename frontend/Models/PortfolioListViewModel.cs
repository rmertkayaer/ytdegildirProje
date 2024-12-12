using System.Text.Json.Serialization;

namespace frontend.Models
{
    public class PortfolioListViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }

        [JsonPropertyName("purchase")]
        public decimal Purchase { get; set; }

        [JsonPropertyName("lastDiv")]
        public decimal LastDiv { get; set; }

        [JsonPropertyName("industry")]
        public string Industry { get; set; }

        [JsonPropertyName("marketCap")]
        public decimal MarketCap { get; set; }
    }
} 