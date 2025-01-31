using StocksApp.Application.DTO.Stocks;

namespace StocksApp.Application.Interfaces.Stocks.Watchlists
{
    public interface IAddToWatchlistService
    {
        Task<WatchlistItemResponse> AddToWatchlistAsync(string userId, Guid watchlistId, string symbol);
    }
}
