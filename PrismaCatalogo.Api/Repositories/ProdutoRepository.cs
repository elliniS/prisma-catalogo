using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PrismaCatalogo.Api.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _context.Set<Produto>()
                .Include(p => p.ProdutosFilhos)
                .Include(p => p.Fotos)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Produto> GetAsync(Expression<Func<Produto, bool>> predicate)
        {
            return await _context.Set<Produto>()
                .Include(p => p.ProdutosFilhos)
                .ThenInclude(pf => pf.Cor)
                .Include(p => p.ProdutosFilhos)
                .ThenInclude(produtoFilho => produtoFilho.Tamanho)
                .Include(p => p.Fotos)
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);
        }
    }
}
