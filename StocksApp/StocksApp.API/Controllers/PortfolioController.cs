using Microsoft.AspNetCore.Mvc;
using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Portfolios;
using System.Security.Claims;

namespace StocksApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IGetPortfolioService _getPortfolioService;
        private readonly ICreatePortfolioService _createPortfolioService;
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(IGetPortfolioService getPortfolioService, ICreatePortfolioService createPortfolioService, ILogger<PortfolioController> logger)
        {
            _getPortfolioService = getPortfolioService;
            _createPortfolioService = createPortfolioService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPortfolios()
        {
            // Get the user id from the JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var portfolios = await _getPortfolioService.GetPortfoliosAsync(userId);
                return Ok(portfolios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolio");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting portfolio");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPortfolioById(Guid id)
        {
            // Get the user id from the JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var portfolio = await _getPortfolioService.GetPortfolioByIdAsync(id);

                if (portfolio == null)
                {
                    return NotFound();
                }

                if (portfolio.UserId != userId)
                {
                    return NotFound();
                }

                return Ok(portfolio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolio");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting portfolio");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePortfolio([FromBody] CreatePortfolioRequest request)
        {
            // Get the user id from the JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            try
            {
                var portfolio = await _createPortfolioService.CreatePortfolioAsync(userId, request.Name);
                return CreatedAtAction(nameof(GetPortfolioById), new { id = portfolio.Id }, portfolio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating portfolio");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating portfolio");
            }
        }
    }
}
