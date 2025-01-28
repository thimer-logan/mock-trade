using Microsoft.AspNetCore.Mvc;
using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Transactions;
using System.Security.Claims;

namespace StocksApp.API.Controllers
{
    [Route("api/portfolio/{id}/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IGetTransactionService _getTransactionService;
        private readonly IBuyStockService _buyStockService;
        private readonly ISellStockService _sellStockService;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(IGetTransactionService getTransactionService, IBuyStockService buyStockService, ISellStockService sellStockService, ILogger<TransactionController> logger)
        {
            _getTransactionService = getTransactionService;
            _buyStockService = buyStockService;
            _sellStockService = sellStockService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromRoute] Guid id)
        {
            // Get the user id from the JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var transactions = await _getTransactionService.GetTransactionsAsync(userId, id);
            return Ok(transactions);
        }

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransactionById([FromRoute] Guid id, [FromRoute] Guid transactionId)
        {
            // Get the user id from the JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var transaction = await _getTransactionService.GetTransactionByIdAsync(userId, id, transactionId);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpPost]
        [Route("buy")]
        public async Task<IActionResult> BuyStock([FromRoute] Guid id, [FromBody] BuyStockRequest buyStockReq)
        {
            // Get the user id from the JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var transaction = await _buyStockService.BuyStockAsync(userId, id, buyStockReq);
            return CreatedAtAction(nameof(GetTransactionById), new { id, transactionId = transaction.Id }, transaction);
        }

        [HttpPost]
        [Route("sell")]
        public async Task<IActionResult> SellStock([FromRoute] Guid id, [FromBody] SellStockRequest sellStockReq)
        {
            // Get the user id from the JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var transaction = await _sellStockService.SellStockAsync(userId, id, sellStockReq);
            return CreatedAtAction(nameof(GetTransactionById), new { id, transactionId = transaction.Id }, transaction);
        }
    }
}
