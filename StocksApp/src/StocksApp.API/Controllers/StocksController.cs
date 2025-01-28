using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StocksApp.Application.Interfaces.Finnhub;

namespace StocksApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IFinnhubStockPriceQuoteService _quoteService;
        private readonly IFinnhubStocksService _stockService;
        private readonly IFinnhubCompanyProfileService _companyService;
        private readonly IFinnhubStockSearchService _stockSearchService;
        private readonly ILogger<StocksController> _logger;

        public StocksController(IFinnhubStockPriceQuoteService quoteService, IFinnhubStocksService stockService, IFinnhubCompanyProfileService companyService, IFinnhubStockSearchService stockSearchService, ILogger<StocksController> logger)
        {
            _quoteService = quoteService;
            _stockService = stockService;
            _companyService = companyService;
            _stockSearchService = stockSearchService;
            _logger = logger;
        }

        [HttpGet("quote/{symbol}")]
        public async Task<IActionResult> GetQuote(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
                return BadRequest("Symbol is required.");

            try
            {
                var quote = await _quoteService.GetStockPriceQuote(symbol);
                if (quote == null)
                    return NotFound("Quote not found.");

                var quoteJson = JsonConvert.SerializeObject(quote);
                return Ok(quoteJson);
            }
            catch (Exception ex)
            {
                // Log exception, return a friendly error
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("company/{symbol}")]
        public async Task<IActionResult> GetCompany(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
                return BadRequest("Symbol is required.");
            try
            {
                var company = await _companyService.GetCompanyProfile(symbol);
                if (company == null)
                    return NotFound("Company not found.");
                var companyJson = JsonConvert.SerializeObject(company);
                return Ok(companyJson);
            }
            catch (Exception ex)
            {
                // Log exception, return a friendly error
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("stocks")]
        public async Task<IActionResult> GetStocks()
        {
            try
            {
                var stocks = await _stockService.GetStocks();
                if (stocks == null)
                    return NotFound("Stocks not found.");
                var stocksJson = JsonConvert.SerializeObject(stocks);
                return Ok(stocksJson);
            }
            catch (Exception ex)
            {
                // Log exception, return a friendly error
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("search/{symbol}")]
        public async Task<IActionResult> SearchStocks(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
                return BadRequest("Symbol is required.");
            try
            {
                var stocks = await _stockSearchService.SearchStocks(symbol);
                if (stocks == null)
                    return NotFound("Stocks not found.");
                var stocksJson = JsonConvert.SerializeObject(stocks);
                return Ok(stocksJson);
            }
            catch (Exception ex)
            {
                // Log exception, return a friendly error
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
