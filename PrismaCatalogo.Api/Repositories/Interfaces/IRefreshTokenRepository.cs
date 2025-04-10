using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Repositories.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        IEnumerable<RefreshToken> Delete(IEnumerable<RefreshToken> refreshTokens);
        void SaveChanges();
    }
}
