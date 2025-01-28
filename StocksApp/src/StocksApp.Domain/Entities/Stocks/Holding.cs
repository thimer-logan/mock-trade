using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StocksApp.Domain.Entities.Stocks
{
    /// <summary>
    /// Represents a position (i.e., stock shares) a user holds in a given portfolio.
    /// </summary>
    public class Holding
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The portfolio this holding belongs to.
        /// </summary>
        public Guid PortfolioId { get; set; }

        [ForeignKey(nameof(PortfolioId))]
        public virtual Portfolio Portfolio { get; set; } = null!;

        /// <summary>
        /// Ticker symbol of the stock (e.g., "AAPL").
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// How many shares are held (can be fractional if your app allows it).
        /// </summary>
        [Precision(18, 6)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// The average price paid per share.
        /// Updated as the user buys more shares.
        /// </summary>
        [Precision(18, 2)]
        public decimal AverageCostBasis { get; set; }

        /// <summary>
        /// When this holding was first created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last time this holding was updated (e.g., after a buy/sell).
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
