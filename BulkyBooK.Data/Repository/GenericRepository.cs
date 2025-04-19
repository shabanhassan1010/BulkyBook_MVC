using BulkyBook.Data.DBContext;
using BulkyBook.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Data.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(ApplicationDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }
        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
