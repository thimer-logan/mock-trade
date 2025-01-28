using System.ComponentModel.DataAnnotations;

namespace StocksApp.Application.DTO.Stocks
{
    public class CreatePortfolioRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
    }
}
