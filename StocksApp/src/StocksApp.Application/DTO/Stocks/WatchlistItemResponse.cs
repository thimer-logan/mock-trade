using StocksApp.Domain.Entities.Stocks;

namespace StocksApp.Application.DTO.Stocks
{
    public class WatchlistItemResponse
    {
        public string Ticker { get; set; } = string.Empty;
    }

    public static class WatchlistItemResponseExtensions
    {
        public static WatchlistItemResponse ToWatchlistItemResponse(this WatchlistItem item)
        {
            return new WatchlistItemResponse
            {
                Ticker = item.Ticker
            };
        }
    }
}
