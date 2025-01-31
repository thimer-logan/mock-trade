using StocksApp.Application.Interfaces.Stocks.Watchlists;
using StocksApp.Domain.Interfaces;

namespace StocksApp.Application.Services.Stocks.Watchlists
{
    public class DeleteWatchlistService : IDeleteWatchlistService
    {
        private readonly IWatchlistRepository _watchlistRepository;

        public DeleteWatchlistService(IWatchlistRepository watchlistRepository)
        {
            _watchlistRepository = watchlistRepository;
        }

        public async Task<bool> DeleteWatchlistAsync(string userId, Guid watchlistId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }
            if (watchlistId == Guid.Empty)
            {
                throw new ArgumentException("Watchlist ID cannot be empty", nameof(watchlistId));
            }

            return await _watchlistRepository.DeleteWatchlistAsync(watchlistId);
        }
    }
}
