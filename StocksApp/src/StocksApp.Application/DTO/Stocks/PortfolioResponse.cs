using StocksApp.Domain.Entities.Stocks;

namespace StocksApp.Application.DTO.Stocks
{
    /// <summary>
    /// Represents the response for a portfolio
    /// </summary>
    public class PortfolioResponse
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Name { get; set; } = "Default Portfolio";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public ICollection<HoldingResponse> Holdings { get; set; } = new List<HoldingResponse>();

        public ICollection<TransactionResponse> Transactions { get; set; } = new List<TransactionResponse>();
    }

    public static class PortfolioResponseExtensions
    {
        public static PortfolioResponse ToPortfolioResponse(this Portfolio portfolio)
        {
            return new PortfolioResponse
            {
                Id = portfolio.Id,
                UserId = portfolio.UserId,
                Name = portfolio.Name,
                CreatedAt = portfolio.CreatedAt,
                LastUpdated = portfolio.LastUpdated,
                Holdings = portfolio.Holdings.Select(h => h.ToHoldingResponse()).ToList(),
                Transactions = portfolio.Transactions.Select(t => t.ToTransactionResponse()).ToList()
            };
        }
    }
}
