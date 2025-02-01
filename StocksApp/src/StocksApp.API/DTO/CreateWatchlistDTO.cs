using System.ComponentModel.DataAnnotations;

namespace StocksApp.API.DTO
{
    public class CreateWatchlistDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
