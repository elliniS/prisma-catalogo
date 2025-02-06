using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;

namespace PrismaCatalogo.Web.Services.Interfaces
{
    public interface IUsuarioService : IService<UsuarioViewModel>
    {
        Task<UsuarioViewModel> Login(UsuarioLoginViewModel usuarioViewModel);
    }
}