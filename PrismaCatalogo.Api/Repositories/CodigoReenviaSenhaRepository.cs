using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.Repositories.Interfaces;

namespace PrismaCatalogo.Api.Repositories
{
    public class CodigoReenviaSenhaRepository : Repository<CodigoReenviaSenha>, ICodigoReenviaSenhaRepository
    {
        public CodigoReenviaSenhaRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
