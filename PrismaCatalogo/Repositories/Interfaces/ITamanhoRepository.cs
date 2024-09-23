using PrismaCatalogo.Models;

namespace PrismaCatalogo.Repositories.Interfaces
{
    public interface ITamanhoRepository
    {
        IEnumerable<Tamanho> Tamanhos { get; }
    }
}
