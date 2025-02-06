using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Services.Interfaces
{
    public interface ITokenService
    {
        string GenereteToken(Usuario usuario);
    }
}
