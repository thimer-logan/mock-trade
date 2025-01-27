using StocksApp.Application.DTO.Stocks;

namespace StocksApp.Application.Interfaces.Stocks.Transactions
{
    public interface ISellStockService
    {
        Task<TransactionResponse> SellStockAsync(string userId, Guid portfolioId, SellStockRequest sellStockReq);
    }
}
