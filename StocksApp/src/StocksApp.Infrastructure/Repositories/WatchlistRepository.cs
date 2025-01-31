using Microsoft.EntityFrameworkCore;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Interfaces;
using StocksApp.Infrastructure.DbContext;

namespace StocksApp.Infrastructure.Repositories
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private readonly AppDbContext _dbContext;

        public WatchlistRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WatchlistItem> AddToWatchlistAsync(WatchlistItem item)
        {
            _dbContext.WatchlistItems.Add(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<Watchlist> CreateWatchlistAsync(Watchlist watchlist)
        {
            _dbContext.Watchlist.Add(watchlist);
            await _dbContext.SaveChangesAsync();
            return watchlist;
        }

        public async Task<bool> DeleteWatchlistAsync(Guid watchlistId)
        {
            var watchlist = await _dbContext.Watchlist.FindAsync(watchlistId);
            if (watchlist == null)
            {
                return false;
            }

            _dbContext.Watchlist.Remove(watchlist);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Watchlist?> GetWatchlistByIdAsync(Guid watchlistId)
        {
            return await _dbContext.Watchlist
                .Include(w => w.Items)
                .Where(w => w.Id == watchlistId)
                .FirstOrDefaultAsync();
        }

        public async Task<WatchlistItem?> GetWatchlistItemAsync(Guid watchlistId, string symbol)
        {
            return await _dbContext.WatchlistItems
                .Where(w => w.WatchlistId == watchlistId && w.Ticker == symbol)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<WatchlistItem>> GetWatchlistItemsAsync(Guid watchlistId)
        {
            return await _dbContext.WatchlistItems
                .Where(w => w.WatchlistId == watchlistId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Watchlist>> GetWatchlistsAsync(string userId)
        {
            return await _dbContext.Watchlist
                .Include(w => w.Items)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> RemoveFromWatchlistAsync(Guid watchlistId, string symbol)
        {
            // Find the item in the watchlist.
            var item = await _dbContext.WatchlistItems
                .Where(w => w.WatchlistId == watchlistId && w.Ticker == symbol)
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return false;
            }

            _dbContext.WatchlistItems.Remove(item);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
