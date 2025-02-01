using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Watchlists;
using StocksApp.Domain.Interfaces;

namespace StocksApp.Application.Services.Stocks.Watchlists
{
    public class GetWatchlistsService : IGetWatchlistsService
    {
        private readonly IWatchlistRepository _watchlistRepository;

        public GetWatchlistsService(IWatchlistRepository watchlistRepository)
        {
            _watchlistRepository = watchlistRepository;
        }

        public async Task<WatchlistResponse?> GetWatchlistByIdAsync(string userId, Guid watchlistId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }
            if (watchlistId == Guid.Empty)
            {
                throw new ArgumentException("Watchlist ID cannot be empty", nameof(watchlistId));
            }

            var watchlist = await _watchlistRepository.GetWatchlistByIdAsync(watchlistId);
            if (watchlist == null)
            {
                return null;
            }
            if (watchlist.UserId != userId)
            {
                throw new UnauthorizedAccessException("User is not authorized to access this watchlist");
            }

            return watchlist?.ToWatchlistResponse();
        }

        public Task<WatchlistItemResponse?> GetWatchlistItemAsync(string userId, Guid watchlistId, string symbol)
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

            return _watchlistRepository.GetWatchlistItemAsync(watchlistId, symbol)
                .ContinueWith(t => t.Result?.ToWatchlistItemResponse());
        }

        public Task<IEnumerable<WatchlistResponse>> GetWatchlistsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            return _watchlistRepository.GetWatchlistsAsync(userId)
                .ContinueWith(t => t.Result.Select(w => w.ToWatchlistResponse()));
        }
    }
}
