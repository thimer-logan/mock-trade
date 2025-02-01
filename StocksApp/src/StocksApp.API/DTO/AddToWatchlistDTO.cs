using System.ComponentModel.DataAnnotations;

namespace StocksApp.API.DTO
{
    public class AddToWatchlistDTO
    {
        [Required]
        public string Symbol { get; set; }
    }
}
