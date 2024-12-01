using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Context;

namespace PrismaCatalogo.Api.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ITamanhoRepository? _tamanho;
        private ICorRepository? _cor;
        private ICategoriaRepository _categoria;

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

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose() 
        { 
            _context.Dispose();
        }
    }
}
