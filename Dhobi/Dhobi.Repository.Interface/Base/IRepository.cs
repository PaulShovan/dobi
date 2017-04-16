using System.Linq;

namespace Dhobi.Repository.Interface.Base
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        bool Add(T entity);
        void UpdateEntity(T entity);
        void Delete(T entity);
        void Delete(string id);
    }
}
