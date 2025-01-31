using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Watchlists;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Interfaces;

namespace StocksApp.Application.Services.Stocks.Watchlists
{
    public class AddToWatchlistService : IAddToWatchlistService
    {
        private readonly IWatchlistRepository _watchlistRepository;

        public AddToWatchlistService(IWatchlistRepository watchlistRepository)
        {
            _watchlistRepository = watchlistRepository;
        }

        public async Task<WatchlistItemResponse> AddToWatchlistAsync(string userId, Guid watchlistId, string symbol)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be empty", nameof(userId));
            }
            if (watchlistId == Guid.Empty)
            {
                throw new ArgumentException("Watchlist ID cannot be empty", nameof(watchlistId));
            }
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("Symbol cannot be empty", nameof(symbol));
            }

            // Check if the user is authorized to add to the watchlist
            var watchlist = await _watchlistRepository.GetWatchlistByIdAsync(watchlistId);
            if (watchlist == null)
            {
                throw new ArgumentException("Watchlist not found", nameof(watchlistId));
            }
            if (watchlist.UserId != userId)
            {
                throw new UnauthorizedAccessException("User is not authorized to access this watchlist");
            }

            // Check if the symbol is already in the watchlist
            var existingWatchlistItem = await _watchlistRepository.GetWatchlistItemAsync(watchlistId, symbol);
            if (existingWatchlistItem != null)
            {
                throw new ArgumentException("Symbol already exists in the watchlist", nameof(symbol));
            }

            var watchlistItem = new WatchlistItem
            {
                WatchlistId = watchlistId,
                Ticker = symbol
            };

            var addedWatchlistItem = await _watchlistRepository.AddToWatchlistAsync(watchlistItem);
            return addedWatchlistItem.ToWatchlistItemResponse();
        }
    }
}
