using StocksApp.Domain.Entities.Stocks;

namespace StocksApp.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetTransactionByIdAsync(Guid id);
        Task<IEnumerable<Transaction>> GetTransactionsByPortfolioAsync(Guid portfolioId);
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
    }
}
