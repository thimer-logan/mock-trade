using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StocksApp.Domain.Entities.Stocks
{
    /// <summary>
    /// Records a buy or sell action for a specific stock.
    /// </summary>
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The portfolio this transaction is associated with.
        /// </summary>
        public Guid PortfolioId { get; set; }

        [ForeignKey(nameof(PortfolioId))]
        public virtual Portfolio Portfolio { get; set; } = null!;

        /// <summary>
        /// Ticker symbol for the transaction (e.g., "AAPL").
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if this is a Buy or Sell.
        /// </summary>
        [Required]
        public TransactionType Type { get; set; }

        /// <summary>
        /// Number of shares bought or sold.
        /// </summary>
        [Required]
        [Precision(18, 2)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Price per share at which the transaction executed.
        /// Typically fetched from Finnhub at the time of buy/sell.
        /// </summary>
        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        /// <summary>
        /// Timestamp of when the transaction took place.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Simple enum to distinguish buy vs sell. Could also be a string column.
    /// </summary>
    public enum TransactionType
    {
        Buy,
        Sell
    }
}
