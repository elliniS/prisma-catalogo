using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace PrismaCatalogo.Api.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            return await _context.Set<Categoria>().AsNoTracking()
            .Include(c => c.CategoriaPai)
            .Include(c => c.CategoriasFilhas)
            .ToListAsync();
        }

        //public async  Task<Categoria?> GetAsync(Expression<Func<Categoria, bool>> predicate)
        //{
        //    return await _context.Set<Categoria>().AsNoTracking()
        //    .Include(c => c.CategoriaPai)
        //    .Include(c => c.CategoriasFilhas)
        //    .Where(predicate)
        //    .FirstOrDefaultAsync();
        //}

        public async Task<IEnumerable<Categoria>> GetCategoriasMesmoNivel(int? idPai)
        {
            IEnumerable<Categoria> categorias = null;

            if (idPai != null)
            {
                 categorias = await _context.Set<Categoria>().AsNoTracking()
                .Include(c => c.CategoriaPai)
                .Include(c => c.CategoriasFilhas)
                .Where(c => c.IdPai == idPai)
                .ToListAsync();

                //categorias =categoria != null ? categoria.CategoriasFilhas : new List<Categoria>();
            }
            else
            {
                categorias = await _context.Set<Categoria>().AsNoTracking()
                .Include(c => c.CategoriaPai)
                .Include(c => c.CategoriasFilhas)
                .Where(c => c.IdPai == null)
                .ToListAsync();


            }
            return categorias;
        }
    }
}
