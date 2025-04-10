using PrismaCatalogo.Api.Models;
using System.Security.Claims;

namespace PrismaCatalogo.Api.Services.Interfaces
{
    public interface ITokenService
    {
        string GenereteToken(Usuario usuario);
        string GenereteToken(IEnumerable<Claim> claims);
        Task<string> GetRefreshToken(int usuarioId);
        string GenereteRefreshToken();
        Task<string> SaveRefreshToken(int usuarioId, string refreshToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<string> DeleteRefreshToken(int usuarioId);
    }
}
