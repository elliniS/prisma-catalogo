using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Repositories
{
    public class TamanhoRepository : Repository<Tamanho>, ITamanhoRepository
    {
        public TamanhoRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
