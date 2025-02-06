using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
