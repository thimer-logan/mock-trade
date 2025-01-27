using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StocksApp.Application.DTO.Stocks
{
    public class BuyStockRequest
    {
        /// <summary>
        /// Ticker symbol for the transaction (e.g., "AAPL").
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// Number of shares bought or sold.
        /// </summary>
        [Required]
        [Precision(18, 2)]
        public decimal Quantity { get; set; }
    }
}
