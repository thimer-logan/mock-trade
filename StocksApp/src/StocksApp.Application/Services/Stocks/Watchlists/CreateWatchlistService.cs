using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Watchlists;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Interfaces;

namespace StocksApp.Application.Services.Stocks.Watchlists
{
    public class CreateWatchlistService : ICreateWatchlistService
    {
        private readonly IWatchlistRepository _watchlistRepository;

        public CreateWatchlistService(IWatchlistRepository watchlistRepository)
        {
            _watchlistRepository = watchlistRepository;
        }

        public async Task<WatchlistResponse> CreateWatchlistAsync(string userId, string name)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be empty", nameof(userId));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty", nameof(name));
            }

            var watchlist = new Watchlist
            {
                UserId = userId,
                Name = name
            };

            var addedWatchlist = await _watchlistRepository.CreateWatchlistAsync(watchlist);
            return addedWatchlist.ToWatchlistResponse();
        }
    }
}
