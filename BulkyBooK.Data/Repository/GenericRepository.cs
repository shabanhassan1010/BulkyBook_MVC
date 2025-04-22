using BulkyBook.Data.DBContext;
using BulkyBook.Data.IRepository;
using Microsoft.EntityFrameworkCore;


namespace BulkyBook.Data.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _context;
        public GenericRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entity)
        {
            _context.Set<T>().RemoveRange(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
