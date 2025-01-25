using System.ComponentModel.DataAnnotations;

namespace StocksApp.Domain.Entities.Stocks
{
    /// <summary>
    /// A user's portfolio. Tracks which stocks they own (Holdings)
    /// and any buy/sell transactions (Transactions).
    /// </summary>
    public class Portfolio
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The ASP.NET Identity user ID or your custom user ID.
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Optional name for the portfolio (e.g., "Main Portfolio").
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; } = "Default Portfolio";

        /// <summary>
        /// When this portfolio was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last time the portfolio was updated.
        /// Could be used when new transactions happen.
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property for the stocks currently held in this portfolio.
        /// </summary>
        public virtual ICollection<Holding> Holdings { get; set; } = new List<Holding>();

        /// <summary>
        /// Navigation property for all buy/sell transactions in this portfolio.
        /// </summary>
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
