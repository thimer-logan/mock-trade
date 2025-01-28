using Microsoft.EntityFrameworkCore;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Interfaces;
using StocksApp.Infrastructure.DbContext;

namespace StocksApp.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _dbContext;

        public TransactionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(Guid id)
        {
            return await _dbContext.Transactions.FindAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByPortfolioAsync(Guid portfolioId)
        {
            return await _dbContext.Transactions
                .Where(t => t.PortfolioId == portfolioId)
                .ToListAsync();
        }
    }
}
