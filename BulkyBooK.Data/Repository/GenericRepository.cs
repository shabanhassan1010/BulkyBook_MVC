using BulkyBook.Data.DBContext;
using BulkyBook.Data.IRepository;
using BulkyBook.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace BulkyBook.Data.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _context;
        protected readonly DbSet<T> _dbSet;
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

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }   

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
