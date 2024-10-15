using PrismaCatalogo.Context;
using PrismaCatalogo.Models;
using PrismaCatalogo.Repositories.Interfaces;

namespace PrismaCatalogo.Repositories
{
    public class CorRepository : ICorRepository
    {
        private readonly ApplicationDbContext _context;

        public CorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Cor> Cores => _context.Cores;
    }
}
