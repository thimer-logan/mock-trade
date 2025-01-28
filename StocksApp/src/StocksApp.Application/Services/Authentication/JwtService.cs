using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StocksApp.Application.DTO.Authentication;
using StocksApp.Application.Interfaces.Authentication;
using StocksApp.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StocksApp.Application.Services.Authentication
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthenticationResponse CreateJwtToken(ApplicationUser user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));

            Claim[] claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT unique id
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()), // Issued at
                new Claim(ClaimTypes.NameIdentifier, user.Email.ToString()), // Unique name identifier of the user
                new Claim(ClaimTypes.Name, user.FirstName.ToString()), // Name of the user
                new Claim(ClaimTypes.Email, user.Email.ToString()), // Email of the user
            ];

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials signingCredentials = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken tokenGenerator = new(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );
            JwtSecurityTokenHandler tokenHandler = new();
            string token = tokenHandler.WriteToken(tokenGenerator);

            return new AuthenticationResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = token,
                RefreshToken = GenerateRefreshToken(),
                Expiration = expiration,
                RefreshExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["RefreshToken:EXPIRATION_MINUTES"]))
            };
        }

        private static string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[64];
            RandomNumberGenerator.Create().GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            };
            JwtSecurityTokenHandler tokenHandler = new();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            if (validatedToken is not JwtSecurityToken jwtSecurityToken || jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
