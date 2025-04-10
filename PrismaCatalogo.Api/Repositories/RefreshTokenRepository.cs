using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Models;
using System.Xml.Linq;

namespace PrismaCatalogo.Api.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<RefreshToken>  Delete(IEnumerable<RefreshToken> entity)
        {
            _context.Set<RefreshToken>().RemoveRange(entity);
            return entity;
        }

        public async void SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
