using BookManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.DataAccess;

public interface IAppDbContext
{
    DbSet<Book> Books { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}