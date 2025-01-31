namespace StocksApp.Application.Interfaces.Stocks.Watchlists
{
    public interface IDeleteWatchlistService
    {
        Task<bool> DeleteWatchlistAsync(string userId, Guid watchlistId);
    }
}
