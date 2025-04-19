using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Data.DBContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base
            (options)
        {

        }
        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { ID = 1, Name = "Action", DisplayOrder = 1 },
                new Category { ID = 2, Name = "Comedy", DisplayOrder = 2 },
                new Category { ID = 3, Name = "Derama", DisplayOrder = 3 },
                new Category { ID = 4, Name = "History", DisplayOrder = 4 }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
