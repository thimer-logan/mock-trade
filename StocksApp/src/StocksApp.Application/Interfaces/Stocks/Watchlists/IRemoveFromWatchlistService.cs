namespace StocksApp.Application.Interfaces.Stocks.Watchlists
{
    public interface IRemoveFromWatchlistService
    {
        Task<bool> RemoveFromWatchlistAsync(string userId, Guid watchlistId, string symbol);
    }
}
