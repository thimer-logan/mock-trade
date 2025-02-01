using Microsoft.AspNetCore.Mvc;
using StocksApp.API.DTO;
using StocksApp.Application.Interfaces.Stocks.Watchlists;
using System.Security.Claims;

namespace StocksApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistController : ControllerBase
    {
        private readonly ICreateWatchlistService _createWatchlistService;
        private readonly IGetWatchlistsService _getWatchlistsService;
        private readonly IAddToWatchlistService _addToWatchlistService;
        private readonly IDeleteWatchlistService _deleteWatchlistService;
        private readonly IRemoveFromWatchlistService _removeFromWatchlistService;

        public WatchlistController(ICreateWatchlistService createWatchlistService,
            IGetWatchlistsService getWatchlistsService,
            IAddToWatchlistService addToWatchlistService,
            IDeleteWatchlistService deleteWatchlistService,
            IRemoveFromWatchlistService removeFromWatchlistService)
        {
            _createWatchlistService = createWatchlistService;
            _getWatchlistsService = getWatchlistsService;
            _addToWatchlistService = addToWatchlistService;
            _deleteWatchlistService = deleteWatchlistService;
            _removeFromWatchlistService = removeFromWatchlistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWatchlists()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var watchlists = await _getWatchlistsService.GetWatchlistsAsync(userId);
            return Ok(watchlists);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWatchlist([FromBody] CreateWatchlistDTO createWatchlistDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var watchlist = await _createWatchlistService.CreateWatchlistAsync(userId, createWatchlistDTO.Name);
            return CreatedAtAction(nameof(GetWatchlistById), new { watchlistId = watchlist.Id }, watchlist);
        }

        [HttpGet("{watchlistId}")]
        public async Task<IActionResult> GetWatchlistById([FromRoute] Guid watchlistId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var watchlist = await _getWatchlistsService.GetWatchlistByIdAsync(userId, watchlistId);
            if (watchlist == null)
            {
                return NotFound();
            }
            return Ok(watchlist);
        }

        [HttpDelete("{watchlistId}")]
        public async Task<IActionResult> DeleteWatchlist([FromRoute] Guid watchlistId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            await _deleteWatchlistService.DeleteWatchlistAsync(userId, watchlistId);
            return NoContent();
        }

        [HttpPost("{watchlistId}/add")]
        public async Task<IActionResult> AddToWatchlist([FromRoute] Guid watchlistId, [FromBody] AddToWatchlistDTO addToWatchlistDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var watchlistItem = await _addToWatchlistService.AddToWatchlistAsync(userId, watchlistId, addToWatchlistDTO.Symbol);
            return CreatedAtAction(
                nameof(GetWatchlistItem),
                new { watchlistId, symbol = watchlistItem.Ticker },
                watchlistItem
            );
        }

        [HttpGet("{watchlistId}/items/{symbol}")]
        public async Task<IActionResult> GetWatchlistItem([FromRoute] Guid watchlistId, [FromRoute] string symbol)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var watchlistItem = await _getWatchlistsService.GetWatchlistItemAsync(userId, watchlistId, symbol);
            if (watchlistItem == null)
            {
                return NotFound();
            }
            return Ok(watchlistItem);
        }

        [HttpDelete("{watchlistId}/items/{symbol}")]
        public async Task<IActionResult> RemoveFromWatchlist([FromRoute] Guid watchlistId, [FromRoute] string symbol)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            await _removeFromWatchlistService.RemoveFromWatchlistAsync(userId, watchlistId, symbol);
            return NoContent();
        }
    }
}
