using BulkyBook.Data.DBContext;
using BulkyBook.Data.Repository;

namespace BulkyBook.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext context;
        public CategoryRepository Categories { get; private set; }
        public ProductRepository Products { get; private set; }
        public CompanyRepository Companies { get; private set; }
        public ApplicationUserRepository applicationUserRepository { get; private set; }
        public ShoppingCartRepository shoppingCartRepository { get; private set; }
        public UnitOfWork(ApplicationDBContext context)
        {
            this.context = context;
            Categories = new CategoryRepository(context);
            Products = new ProductRepository(context);
            Companies = new CompanyRepository(context);
            shoppingCartRepository = new ShoppingCartRepository(context);
            applicationUserRepository = new ApplicationUserRepository(context);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
