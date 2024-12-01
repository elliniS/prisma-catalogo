using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetCategoriasMesmoNivel(int? idPai);
    }
}
