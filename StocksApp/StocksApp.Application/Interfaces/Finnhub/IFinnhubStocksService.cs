using StocksApp.Domain.ValueObjects;

namespace StocksApp.Application.Interfaces.Finnhub
{
    public interface IFinnhubStocksService
    {
        Task<IEnumerable<Stock>?> GetStocks();
    }
}
