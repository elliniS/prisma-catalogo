namespace PrismaCatalogo.Web.Services.Interfaces
{
    public interface IService<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> FindById(int id);
        Task<T> FindByName(string name);
        Task<T> Create(T t);
        Task<T> Update(int id, T t);
        Task<T> Delete(int id);
    }
}
