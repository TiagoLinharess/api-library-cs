using api_library_cs.Models;
using Microsoft.EntityFrameworkCore;

namespace api_library_cs.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasKey(b => b.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}