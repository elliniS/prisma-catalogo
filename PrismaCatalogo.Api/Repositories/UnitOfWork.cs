using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Context;

namespace PrismaCatalogo.Api.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ITamanhoRepository? _tamanho;
        private ICorRepository? _cor;
        private ICategoriaRepository? _categoria;
        private IProdutoRepository? _produto;
        private IProdutoFilhoRepository? _produtoFilho;
        private IProdutoFotoRepository? _produtoFoto;
        private IProdutoFilhoFotoRepository _produtoFilhoFoto;
        private IUsuarioRepository _usuarioRepository;
        private IRefreshTokenRepository _refreshTokenRepository;
        private IAvaliacaoRepository _avaliacaoRepository;

        public ApplicationDbContext _context;
        
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }


        public ITamanhoRepository TamanhoRepository
        {
            get
            {
                return _tamanho = _tamanho ?? new TamanhoRepository(_context);
            }
        }

        public ICorRepository CorRepository
        {
            get
            {
                return _cor = _cor ?? new CorRepository(_context);
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get 
            {
                return _categoria = _categoria ?? new CategoriaRepository(_context);
            }
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produto = _produto ?? new ProdutoRepository(_context);
            }
        }

        public IProdutoFilhoRepository ProdutoFilhoRepository
        {
            get
            {
                return _produtoFilho = _produtoFilho ?? new ProdutoFilhoRepository(_context);
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IProdutoFotoRepository ProdutoFotoRepository
        {
            get
            {
                return _produtoFoto = _produtoFoto ?? new ProdutoFotoRepository(_context);
            }
        }

        public IProdutoFilhoFotoRepository produtoFilhoFotoRepository
        {
            get
            {
                return _produtoFilhoFoto = _produtoFilhoFoto ?? new ProdutoFilhoFotoRepository(_context);
            }
        }

        public IUsuarioRepository UsuarioRepository
        {
            get
            {
                return _usuarioRepository = _usuarioRepository ?? new UsuarioRepository(_context);
            }
        }

        public IRefreshTokenRepository RefreshTokenRepository
        {
            get
            {
                return _refreshTokenRepository = _refreshTokenRepository ?? new RefreshTokenRepository(_context);
            }
        }

        public IAvaliacaoRepository AvaliacaoRepository
        {
            get
            {
                return _avaliacaoRepository = _avaliacaoRepository ?? new AvaliacaoRepository(_context);
            }
        }

        public void Dispose() 
        { 
            _context.Dispose();
        }
    }
}
