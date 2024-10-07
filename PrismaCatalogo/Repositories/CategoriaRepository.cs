using PrismaCatalogo.Context;
using PrismaCatalogo.Models;
using PrismaCatalogo.Repositories.Interfaces;

namespace PrismaCatalogo.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> Categorias => _context.Categorias;
    }
}
