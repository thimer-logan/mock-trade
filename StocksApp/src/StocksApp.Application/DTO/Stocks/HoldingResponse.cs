using StocksApp.Domain.Entities.Stocks;

namespace StocksApp.Application.DTO.Stocks
{
    /// <summary>
    /// 
    /// </summary>
    public class HoldingResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Ticker { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal AverageCostBasis { get; set; }
    }

    public static class HoldingResponseExtensions
    {
        public static HoldingResponse ToHoldingResponse(this Holding holding)
        {
            return new HoldingResponse
            {
                Ticker = holding.Ticker,
                Quantity = holding.Quantity,
                AverageCostBasis = holding.AverageCostBasis
            };
        }
    }
}
