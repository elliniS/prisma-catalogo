using PrismaCatalogo.Context;
using PrismaCatalogo.Models;
using PrismaCatalogo.Repositories.Interfaces;

namespace PrismaCatalogo.Repositories
{
    public class TamanhoRepository : ITamanhoRepository
    {
        private readonly ApplicationDbContext _context;

        public TamanhoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Tamanho> Tamanhos => _context.Tamanhos;
    }
}
