using BulkyBook.Data.DBContext;
using BulkyBook.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext context;
        public CategoryRepository Categories { get; private set; }
        public UnitOfWork(ApplicationDBContext context)
        {
            this.context = context;
            Categories = new CategoryRepository(context);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
