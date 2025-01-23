using StocksApp.Application.DTO.Authentication;
using StocksApp.Domain.Entities.Identity;
using System.Security.Claims;

namespace StocksApp.Application.Interfaces.Authentication
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}
