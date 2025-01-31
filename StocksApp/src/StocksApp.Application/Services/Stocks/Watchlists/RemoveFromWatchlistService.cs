using StocksApp.Application.Interfaces.Stocks.Watchlists;
using StocksApp.Domain.Interfaces;

namespace StocksApp.Application.Services.Stocks.Watchlists
{
    public class RemoveFromWatchlistService : IRemoveFromWatchlistService
    {
        private readonly IWatchlistRepository _watchlistRepository;

        public RemoveFromWatchlistService(IWatchlistRepository watchlistRepository)
        {
            _watchlistRepository = watchlistRepository;
        }

        public async Task<bool> RemoveFromWatchlistAsync(string userId, Guid watchlistId, string symbol)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }
            if (watchlistId == Guid.Empty)
            {
                throw new ArgumentException("Watchlist ID cannot be empty", nameof(watchlistId));
            }
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("Symbol cannot be empty", nameof(symbol));
            }

            return await _watchlistRepository.RemoveFromWatchlistAsync(watchlistId, symbol);
        }
    }
}
