using BulkyBook.Data.DBContext;
using BulkyBook.Models;


namespace BulkyBook.Data.Repository
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}
