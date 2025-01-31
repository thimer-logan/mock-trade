using StocksApp.Application.DTO.Stocks;

namespace StocksApp.Application.Interfaces.Stocks.Watchlists
{
    public interface ICreateWatchlistService
    {
        Task<WatchlistResponse> CreateWatchlistAsync(string userId, string name);
    }
}
