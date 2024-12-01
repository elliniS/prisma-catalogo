using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Repositories
{
    public class CorRepository : Repository<Cor>, ICorRepository
    {
        public CorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
