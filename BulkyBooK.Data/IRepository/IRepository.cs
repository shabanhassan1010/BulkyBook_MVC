
using BulkyBook.Model;
using System.Linq.Expressions;

namespace BulkyBook.Data.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Update(T entuty);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entity);
        void Add(T entity);
    }
}
