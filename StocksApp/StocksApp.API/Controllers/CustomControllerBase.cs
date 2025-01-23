using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StocksApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {
    }
}
