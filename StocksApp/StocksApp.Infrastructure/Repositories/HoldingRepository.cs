using Microsoft.EntityFrameworkCore;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Exceptions;
using StocksApp.Domain.Interfaces;
using StocksApp.Infrastructure.DbContext;

namespace StocksApp.Infrastructure.Repositories
{
    public class HoldingRepository : IHoldingRepository
    {
        private readonly AppDbContext _dbContext;

        public HoldingRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Holding> CreateHoldingAsync(Holding holding)
        {
            holding.CreatedAt = DateTime.UtcNow;
            holding.LastUpdated = DateTime.UtcNow;

            _dbContext.Holdings.Add(holding);
            await _dbContext.SaveChangesAsync();
            return holding;
        }

        public async Task<bool> DeleteHoldingAsync(Guid id)
        {
            var holding = await _dbContext.Holdings.FindAsync(id);
            if (holding == null)
            {
                return false;
            }

            _dbContext.Holdings.Remove(holding);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Holding?> GetHoldingByIdAsync(Guid id)
        {
            return await _dbContext.Holdings.FindAsync(id);
        }

        public async Task<Holding?> GetHoldingByTicker(Guid portfolioId, string ticker)
        {
            return await _dbContext.Holdings
                .Where(h => h.PortfolioId == portfolioId && h.Ticker == ticker)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Holding>> GetHoldingsByPortfolioAsync(Guid portfolioId)
        {
            return await _dbContext.Holdings
                .Where(h => h.PortfolioId == portfolioId)
                .ToListAsync();
        }

        public async Task<Holding> UpdateHoldingAsync(Holding holding)
        {
            Holding? existingHolding = await GetHoldingByIdAsync(holding.Id);

            if (existingHolding == null)
            {
                throw new NotFoundException("Holding does not exist.");
            }

            existingHolding.Ticker = holding.Ticker;
            existingHolding.Quantity = holding.Quantity;
            existingHolding.AverageCostBasis = holding.AverageCostBasis;
            existingHolding.LastUpdated = DateTime.UtcNow;

            _dbContext.Holdings.Update(existingHolding);
            await _dbContext.SaveChangesAsync();

            return existingHolding;
        }
    }
}
