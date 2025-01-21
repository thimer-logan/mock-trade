using StocksApp.Domain.ValueObjects;

namespace StocksApp.Application.Interfaces.Finnhub
{
    public interface IFinnhubStockSearchService
    {
        Task<StockSearch?> SearchStocks(string symbol);
    }
}
