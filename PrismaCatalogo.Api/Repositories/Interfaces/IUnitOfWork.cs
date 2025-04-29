namespace PrismaCatalogo.Api.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ITamanhoRepository TamanhoRepository { get; }
        ICorRepository CorRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        IProdutoRepository ProdutoRepository { get; }
        IProdutoFilhoRepository ProdutoFilhoRepository { get; }
        IProdutoFotoRepository ProdutoFotoRepository { get; }
        IProdutoFilhoFotoRepository produtoFilhoFotoRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IAvaliacaoRepository AvaliacaoRepository { get; }
        Task CommitAsync();
    }
}
