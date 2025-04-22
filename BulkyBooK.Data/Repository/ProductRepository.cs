using BulkyBook.Data.DBContext;
using BulkyBook.Model;
using Microsoft.EntityFrameworkCore;


namespace BulkyBook.Data.Repository
{
    public class ProductRepository : GenericRepository<Product>
    {
        private readonly ApplicationDBContext context;
        public ProductRepository(ApplicationDBContext context) : base(context)
        {
            this.context = context;
        }
        public IEnumerable<Product> GetAllIncluding()
        {
            return context.Products.Include(c => c.Category).ToList();
        }
        public Product GetByIdIncluding(int id)
        {
            return context.Products.Include(c => c.Category).FirstOrDefault(c=>c.Id == id);
        }
    }
}
