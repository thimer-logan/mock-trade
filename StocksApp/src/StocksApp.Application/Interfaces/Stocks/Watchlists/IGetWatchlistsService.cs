using StocksApp.Application.DTO.Stocks;

namespace StocksApp.Application.Interfaces.Stocks.Watchlists
{
    public interface IGetWatchlistsService
    {
        Task<IEnumerable<WatchlistResponse>> GetWatchlistsAsync(string userId);

        Task<WatchlistResponse?> GetWatchlistByIdAsync(string userId, Guid watchlistId);

        Task<WatchlistItemResponse?> GetWatchlistItemAsync(string userId, Guid watchlistId, string symbol);
    }
}
