using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Repositories
{
    public class ProdutoFilhoFotoRepository : Repository<ProdutoFilhoFoto>, IProdutoFilhoFotoRepository
    {
        public ProdutoFilhoFotoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
