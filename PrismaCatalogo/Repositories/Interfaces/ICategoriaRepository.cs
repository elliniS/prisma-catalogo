using PrismaCatalogo.Models;

namespace PrismaCatalogo.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> Categorias { get; }
    }
}
