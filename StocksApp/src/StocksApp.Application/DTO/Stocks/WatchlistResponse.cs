using StocksApp.Domain.Entities.Stocks;

namespace StocksApp.Application.DTO.Stocks
{
    public class WatchlistResponse
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = "Default Watchlist";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<WatchlistItemResponse> Items { get; set; } = new List<WatchlistItemResponse>();
    }

    public static class WatchlistResponseExtensions
    {
        public static WatchlistResponse ToWatchlistResponse(this Watchlist watchlist)
        {
            return new WatchlistResponse
            {
                Id = watchlist.Id,
                UserId = watchlist.UserId,
                Name = watchlist.Name,
                CreatedAt = watchlist.CreatedAt,
                Items = watchlist.Items.Select(i => i.ToWatchlistItemResponse()).ToList()
            };
        }
    }
}
