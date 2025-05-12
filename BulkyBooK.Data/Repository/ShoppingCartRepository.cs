using BulkyBook.Data.DBContext;
using BulkyBook.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BulkyBook.Data.Repository
{
    public class ShoppingCartRepository : GenericRepository<ShopingCart>
    {
        private readonly ApplicationDBContext context;
        public ShoppingCartRepository(ApplicationDBContext context) : base(context)
        {
            this.context = context;
        }
        public ShopingCart GetFirstOrDefault(Expression<Func<ShopingCart, bool>> filter)
        {
            return context.ShopingCarts.Include(sc => sc.ApplicationUser).Include(s=>s.Product).FirstOrDefault(filter);
        }
        public IEnumerable<ShopingCart> GetAll(Expression<Func<ShopingCart, bool>>? filter,string? includeProperties = null)
        {
            IQueryable<ShopingCart> query = _dbSet;

            // Apply filter
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Include navigation properties
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
        }
    }
}
