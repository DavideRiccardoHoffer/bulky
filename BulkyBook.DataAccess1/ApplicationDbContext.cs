using BulkyBook.Models1;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess1
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
		public DbSet<CoverType> CoverTypes { get; set; } = null!;
	}
}
