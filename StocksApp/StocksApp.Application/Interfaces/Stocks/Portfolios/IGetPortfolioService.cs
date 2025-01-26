using StocksApp.Application.DTO.Stocks;

namespace StocksApp.Application.Interfaces.Stocks.Portfolios
{
    /// <summary>
    /// Service to get a user's portfolio.
    /// </summary>
    public interface IGetPortfolioService
    {
        /// <summary>
        /// Get a user's portfolio.
        /// </summary>
        /// <param name="portfolioId">Portfolio ID</param>
        /// <returns>User's portfolio</returns>
        Task<PortfolioResponse?> GetPortfolioByIdAsync(string userId, Guid portfolioId);

        /// <summary>
        /// Get all portfolios for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>All portfolios for the user</returns>
        Task<IEnumerable<PortfolioResponse>?> GetPortfoliosAsync(string userId);
    }
}
