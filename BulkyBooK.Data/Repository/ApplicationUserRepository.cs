using BulkyBook.Data.DBContext;
using BulkyBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Data.Repository
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>
    {
        public ApplicationUserRepository(ApplicationDBContext context) : base(context)
        {
            
        }
    }
}
