using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StocksApp.Domain.Entities.Stocks
{
    public class WatchlistItem
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid WatchlistId { get; set; }

        [ForeignKey(nameof(WatchlistId))]
        public virtual Watchlist Watchlist { get; set; } = null!;

        /// <summary>
        /// The ticker symbol (e.g., "TSLA") being watched.
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Ticker { get; set; } = string.Empty;
    }
}
