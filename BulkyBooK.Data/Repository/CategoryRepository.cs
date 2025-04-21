using BulkyBook.Data.DBContext;
using BulkyBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Data.Repository
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}
