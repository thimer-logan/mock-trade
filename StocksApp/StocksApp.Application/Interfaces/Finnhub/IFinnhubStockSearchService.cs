using StocksApp.Domain.ValueObjects;

namespace StocksApp.Application.Interfaces.Finnhub
{
    public interface IFinnhubStockSearchService
    {
        Task<IEnumerable<StockSearch>?> SearchStocks(string symbol);
    }
}
