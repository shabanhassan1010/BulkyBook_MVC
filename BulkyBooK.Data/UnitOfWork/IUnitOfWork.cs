using BulkyBook.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        CategoryRepository Categories { get; }
        CompanyRepository Companies { get; }
        ProductRepository Products { get; }
        ShoppingCartRepository shoppingCartRepository { get; }
        ApplicationUserRepository applicationUserRepository { get; }
        void Save();
    }
}
