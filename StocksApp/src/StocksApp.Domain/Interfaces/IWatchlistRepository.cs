using StocksApp.Domain.Entities.Stocks;

namespace StocksApp.Domain.Interfaces
{
    public interface IWatchlistRepository
    {
        Task<WatchlistItem> AddToWatchlistAsync(WatchlistItem item);

        Task<WatchlistItem?> GetWatchlistItemAsync(Guid watchlistId, string symbol);

        Task<IEnumerable<WatchlistItem>> GetWatchlistItemsAsync(Guid watchlistId);

        Task<Watchlist?> GetWatchlistByIdAsync(Guid watchlistId);

        Task<IEnumerable<Watchlist>> GetWatchlistsAsync(string userId);

        Task<Watchlist> CreateWatchlistAsync(Watchlist watchlist);

        Task<bool> DeleteWatchlistAsync(Guid watchlistId);

        Task<bool> RemoveFromWatchlistAsync(Guid watchlistId, string symbol);
    }
}
