using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Stock
{
    public class UpdateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Sembol en fazla 10 karakter olabilir.")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MaxLength(10, ErrorMessage = "Şirket Adı en fazla 10 karakter olabilir.")]
        public string CompanyName { get; set; } = string.Empty;
        
        [Required]
        [Range(1, 100000000)]
        public decimal Purchase { get; set; }
        
        [Required]
        [Range(0.001, 1000)]
        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Endüstri Adı en fazla 10 karakter olabilir.")]
        public string Industry { get; set; } = string.Empty;

        [Range(1, 1000000000)]
        public long MarketCap { get; set; }
    }
}