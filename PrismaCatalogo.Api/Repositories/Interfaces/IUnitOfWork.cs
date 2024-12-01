namespace PrismaCatalogo.Api.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ITamanhoRepository TamanhoRepository { get; }
        ICorRepository CorRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        Task CommitAsync();
    }
}
