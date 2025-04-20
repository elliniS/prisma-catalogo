using Microsoft.EntityFrameworkCore;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Context;
using System.Linq.Expressions;
using AutoMapper;

namespace PrismaCatalogo.Api.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return  await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<TS>(Expression<Func<T, TS>> select)
        {
           var obj =  await _context.Set<T>().Select(select).ToListAsync();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<TS, T>());
            var mapper = config.CreateMapper();

            return obj.Select(s => mapper.Map<T>(s)).ToList();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetListAsync<TS>(Expression<Func<T, bool>> predicate, Expression<Func<T, TS>> select)
        {
            var obj = await _context.Set<T>().AsNoTracking().Where(predicate).Select(select).ToListAsync();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<TS, T>());
            var mapper = config.CreateMapper();

            return obj.Select(s => mapper.Map<T>(s)).ToList();
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }

        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return entity;
        }
    }
}
