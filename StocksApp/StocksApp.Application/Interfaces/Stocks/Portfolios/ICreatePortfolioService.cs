using StocksApp.Application.DTO.Stocks;

namespace StocksApp.Application.Interfaces.Stocks.Portfolios
{
    /// <summary>
    /// Service for creating a new portfolio for a user.
    /// </summary>
    public interface ICreatePortfolioService
    {
        /// <summary>
        /// Creates a new portfolio for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="portfolioName"></param>
        /// <returns></returns>
        Task<PortfolioResponse> CreatePortfolioAsync(string userId, string portfolioName);
    }
}
