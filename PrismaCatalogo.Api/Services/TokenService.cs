using Microsoft.IdentityModel.Tokens;
using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using PrismaCatalogo.Api.Repositories.Interfaces;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PrismaCatalogo.Api.Repositories;

namespace PrismaCatalogo.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork _refreshTokenRepository;

        public TokenService(IUnitOfWork refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public string GenereteToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name, usuario.NomeUsuario),
                 new Claim(ClaimTypes.Role, usuario.UsuarioTipo.ToString())
            };

            return GenereteToken(claims);
        }

        public string GenereteToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenereteRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidation = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Secret)),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidation, out var securityToken);

            if(securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;

        }

        public async Task<string> SaveRefreshToken(int usuarioId, string refreshToken)
        {
            RefreshToken refreshToken1 = new RefreshToken() { 
                
                UsuarioId =  usuarioId,
                RefreshTokenValue = refreshToken
            };

            //var aaa = await _refreshTokenRepository.GetAllAsync();

            _refreshTokenRepository.RefreshTokenRepository.Create(refreshToken1);
           await _refreshTokenRepository.CommitAsync();

            return "";
        }

        public async Task<string> GetRefreshToken(int usuarioId)
        {
            return (await _refreshTokenRepository.RefreshTokenRepository.GetAsync(r => r.UsuarioId ==  usuarioId)).RefreshTokenValue;
        }

        public async Task<string> DeleteRefreshToken(int usuarioId)
        {
            var refrashTokens = await _refreshTokenRepository.RefreshTokenRepository.GetListAsync(r => r.UsuarioId == usuarioId);

            _refreshTokenRepository.RefreshTokenRepository.Delete(refrashTokens);

            await _refreshTokenRepository.CommitAsync();

            return "";
        }
    }
}
