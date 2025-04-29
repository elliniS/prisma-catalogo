using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PrismaCatalogo.Api.Repositories
{
    public class AvaliacaoRepository : Repository<Avaliacao>, IAvaliacaoRepository
    {
        public AvaliacaoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Avaliacao>> GetListAsync(Expression<Func<Avaliacao, bool>> predicate)
        {
            return await _context.Set<Avaliacao>().AsNoTracking().Where(predicate)
                .Include(a => a.Usuario)
                .ToListAsync();
        }
    }
}
