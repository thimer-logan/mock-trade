using StocksApp.Application.DTO.Stocks;

namespace StocksApp.Application.Interfaces.Stocks.Transactions
{
    public interface IGetTransactionService
    {
        /// <summary>
        /// Get all transactions for a user's portfolio.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="portfolioId"></param>
        /// <returns></returns>
        Task<IEnumerable<TransactionResponse>> GetTransactionsAsync(string userId, Guid portfolioId);

        /// <summary>
        /// Get a single transaction by its Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="portfolioId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TransactionResponse> GetTransactionByIdAsync(string userId, Guid portfolioId, Guid id);
    }
}
