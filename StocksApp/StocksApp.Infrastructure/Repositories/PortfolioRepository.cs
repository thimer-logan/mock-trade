using Microsoft.EntityFrameworkCore;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Interfaces;
using StocksApp.Infrastructure.DbContext;

namespace StocksApp.Infrastructure.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly AppDbContext _dbContext;

        public PortfolioRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<Portfolio?> GetPortfolioAsync(Guid portfolioId)
        {
            // Fetch the portfolio by ID and ensure UserId matches.
            return await _dbContext.Portfolios
                .Include(p => p.Holdings)
                .Include(p => p.Transactions)
                .Where(p => p.Id == portfolioId)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Portfolio>> GetPortfoliosByUserAsync(string userId)
        {
            // Return all portfolios for the user.
            return await _dbContext.Portfolios
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio)
        {
            // We assume the Portfolio entity already has a valid UserId set.
            portfolio.CreatedAt = DateTime.UtcNow;
            portfolio.LastUpdated = DateTime.UtcNow;

            _dbContext.Portfolios.Add(portfolio);
            await _dbContext.SaveChangesAsync();

            return portfolio;
        }
    }
}
