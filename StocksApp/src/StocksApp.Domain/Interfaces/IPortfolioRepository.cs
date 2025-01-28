using StocksApp.Domain.Entities.Stocks;

namespace StocksApp.Domain.Interfaces
{
    /// <summary>
    /// Repository for managing a user's portfolio.
    /// </summary>
    public interface IPortfolioRepository
    {
        /// <summary>
        /// Gets a single Portfolio by its Id, ensuring it matches the given userId.
        /// Returns null if not found.
        /// </summary>
        Task<Portfolio?> GetPortfolioAsync(Guid portfolioId);

        /// <summary>
        /// Gets all portfolios belonging to the specified user.
        /// </summary>
        Task<IEnumerable<Portfolio>> GetPortfoliosByUserAsync(string userId);

        /// <summary>
        /// Creates a new portfolio in the database.
        /// </summary>
        Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio);
    }
}
