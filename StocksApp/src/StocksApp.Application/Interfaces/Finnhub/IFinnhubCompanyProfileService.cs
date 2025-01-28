using StocksApp.Domain.ValueObjects;

namespace StocksApp.Application.Interfaces.Finnhub
{
    public interface IFinnhubCompanyProfileService
    {
        Task<CompanyProfile?> GetCompanyProfile(string symbol);
    }
}
