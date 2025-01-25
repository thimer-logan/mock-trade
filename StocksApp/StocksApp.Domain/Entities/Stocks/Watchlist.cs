using System.ComponentModel.DataAnnotations;

namespace StocksApp.Domain.Entities.Stocks
{
    /// <summary>
    /// A watchlist for tracking stocks the user is interested in.
    /// </summary>
    public class Watchlist
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The user who owns this watchlist (ASP.NET Core Identity user id).
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Friendly name for this watchlist (e.g., "Tech Stocks").
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; } = "Default Watchlist";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property to the watchlist items (tickers).
        /// </summary>
        public virtual ICollection<WatchlistItem> Items { get; set; } = new List<WatchlistItem>();
    }
}
