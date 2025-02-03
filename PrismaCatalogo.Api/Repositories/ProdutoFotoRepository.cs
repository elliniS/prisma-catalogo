using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Repositories
{
    public class ProdutoFotoRepository : Repository<ProdutoFoto>, IProdutoFotoRepository
    {
        public ProdutoFotoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
