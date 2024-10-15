using PrismaCatalogo.Models;

namespace PrismaCatalogo.Repositories.Interfaces
{
    public interface ICorRepository
    {
        IEnumerable<Cor> Cores { get; }
    }
}
