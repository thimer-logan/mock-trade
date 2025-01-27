using StocksApp.Application.DTO.Stocks;

namespace StocksApp.Application.Interfaces.Stocks.Transactions
{
    public interface IBuyStockService
    {
        Task<TransactionResponse> BuyStockAsync(string userId, Guid portfolioId, BuyStockRequest buyStockReq);
    }
}
