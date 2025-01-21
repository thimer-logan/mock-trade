using StocksApp.Domain.ValueObjects;

namespace StocksApp.Application.Interfaces.Finnhub
{
    public interface IFinnhubStockPriceQuoteService
    {
        Task<StockQuote?> GetStockPriceQuote(string symbol);
    }
}
