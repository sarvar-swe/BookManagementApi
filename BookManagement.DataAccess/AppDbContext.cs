using BookManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.DataAccess;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Book> Books { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasKey(e => e.Id);
            
        base.OnModelCreating(modelBuilder);
    }

    Task<int> IAppDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        => base.SaveChangesAsync(cancellationToken);
}
