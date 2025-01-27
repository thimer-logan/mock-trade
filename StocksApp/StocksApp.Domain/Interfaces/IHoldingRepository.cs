using StocksApp.Domain.Entities.Stocks;

namespace StocksApp.Domain.Interfaces
{
    public interface IHoldingRepository
    {
        /// <summary>
        /// Get a single holding by its unique identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Holding?> GetHoldingByIdAsync(Guid id);

        /// <summary>
        /// Get all holdings for a given portfolio.
        /// </summary>
        /// <param name="portfolioId"></param>
        /// <returns></returns>
        Task<IEnumerable<Holding>> GetHoldingsByPortfolioAsync(Guid portfolioId);

        /// <summary>
        /// Get a single holding by its ticker symbol.
        /// </summary>
        /// <param name="portfolioId"></param>
        /// <param name="ticker"></param>
        /// <returns></returns>
        Task<Holding?> GetHoldingByTicker(Guid portfolioId, string ticker);

        /// <summary>
        /// Create a new holding.
        /// </summary>
        /// <param name="holding"></param>
        /// <returns></returns>
        Task<Holding> CreateHoldingAsync(Holding holding);

        /// <summary>
        /// Update an existing holding.
        /// </summary>
        /// <param name="holding"></param>
        /// <returns></returns>
        Task<Holding> UpdateHoldingAsync(Holding holding);

        /// <summary>
        /// Delete a holding by its unique identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteHoldingAsync(Guid id);
    }
}
