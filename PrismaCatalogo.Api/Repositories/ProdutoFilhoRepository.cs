using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PrismaCatalogo.Api.Repositories
{
    public class ProdutoFilhoRepository : Repository<ProdutoFilho>, IProdutoFilhoRepository
    {
        public ProdutoFilhoRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<ProdutoFilho>> GetAllAsync()
        {
            return await _context.Set<ProdutoFilho>()
            .Include(p => p.Cor)
            .Include(p => p.Tamanho)
            .Include(p => p.Fotos)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<ProdutoFilho> GetAsync(Expression<Func<ProdutoFilho, bool>> predicate)
        {
            return await _context.Set<ProdutoFilho>()
                .Include(p => p.Cor)
                .Include(p => p.Tamanho)
                .Include(p => p.Fotos)
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<ProdutoFilho>> GetListAsync(Expression<Func<ProdutoFilho, bool>> predicate)
        {
            return await _context.Set<ProdutoFilho>()
                .Include(p => p.Cor)
                .Include(p => p.Tamanho)
                .Include(p => p.Fotos)
                .AsNoTracking()
                .Where(predicate).ToListAsync();
        }
    }
}
