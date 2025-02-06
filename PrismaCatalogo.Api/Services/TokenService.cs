using Microsoft.IdentityModel.Tokens;
using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace PrismaCatalogo.Api.Services
{
    public class TokenService : ITokenService
    {
        public string GenereteToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usuario.NomeUsuario),
                    new Claim(ClaimTypes.Role, usuario.UsuarioTipo.ToString())
                })
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
