using Microsoft.EntityFrameworkCore;

namespace LibraryAPI
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
    }
}
