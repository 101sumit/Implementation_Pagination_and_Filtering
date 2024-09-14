using Implementation_Pagination_and_Filtering.Models;
using Microsoft.EntityFrameworkCore;

namespace Implementation_Pagination_and_Filtering
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
