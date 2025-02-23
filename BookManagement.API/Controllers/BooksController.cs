using BookManagement.DataAccess;
using BookManagement.Models;
using BookManagement.Models.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddBook(
        [FromBody] AddBookDto bookDto,
        [FromServices] IAppDbContext dbContext)
    {
        var existingBook = await dbContext.Books.FirstOrDefaultAsync(b => 
            b.Title == bookDto.Title &&
            b.Author == bookDto.Author &&
            b.PublicationYear == bookDto.PublicationYear);

        if(existingBook is not null)
        {
            if(existingBook.IsDeleted)
            {
                existingBook.IsDeleted = false;
                existingBook.ModifiedAt = DateTime.Now;

                await dbContext.SaveChangesAsync();
                return Ok(new GetBookDto(existingBook));
            }

            return Conflict("The book has already been added.");
        }

        var book = dbContext.Books.Add(new Book 
        {
            Id = Guid.NewGuid(),
            Title = bookDto.Title,
            PublicationYear = bookDto.PublicationYear,
            Author = bookDto.Author,
            ViewsCount = 0,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        });

        await dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetBook),
            new { id = book.Entity.Id },
            new GetBookDto(book.Entity)
        );
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> AddBooks(
        [FromBody] List<AddBookDto> booksDto,
        [FromServices] IAppDbContext dbContext)
    {
        if(booksDto == null || booksDto.Count == 0)
            return BadRequest("Book list cannot be emoty");

        var existingBooks = await dbContext.Books
            .Select(b => new { b.Title, b.Author})
            .ToListAsync();

        var newBooks = booksDto
            .Where(b => !existingBooks.Any(e => e.Title == b.Title && e.Author == b.Author))
            .Select(b => new Book
            {
                Id = Guid.NewGuid(),
                Title = b.Title,
                PublicationYear = b.PublicationYear,
                Author = b.Author,
                ViewsCount = 0,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            }).ToList();

        if(newBooks.Count == 0)
            return Conflict("All books already exist in the database");

        await dbContext.Books.AddRangeAsync(newBooks);
        await dbContext.SaveChangesAsync();

        return Ok(new { Message = $"{newBooks.Count} new books added successfully!"});
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks(
        [FromServices] IAppDbContext dbContext,
        [FromQuery] string? search,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 15)
    {

        var query = dbContext.Books.AsQueryable();

        if(!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b =>
                !b.IsDeleted && EF.Functions.Like(b.Title ?? "", $"%{search.ToLower()}%"));
        }

        var books = await query
            .Skip(limit * offset)
            .Take(limit)
            .Select(q => new GetBookWithPopularityScoreDto(q))
            .ToListAsync();

        var result = new PaginatedList<GetBookWithPopularityScoreDto>(
            books.OrderByDescending(b => b.PopularityScore),
            query.Count(),
            offset + 1,
            limit);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(
        [FromRoute] Guid id,
        [FromServices] IAppDbContext dbContext)
    {
        var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        if(book is null)
            return NotFound();
        
        book.ViewsCount++;
        await dbContext.SaveChangesAsync();

        return Ok(new GetBookDto(book));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(
        [FromRoute] Guid id,
        [FromServices] IAppDbContext dbContext,
        UpdateBookDto bookDto)
    {
        var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        if(book is null)
            return NotFound();
        
        book.Title = bookDto.Title;
        book.PublicationYear = bookDto.PublicationYear;
        book.Author = bookDto.Author;
        book.ModifiedAt = DateTime.Now;

        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(
        [FromRoute] Guid id,
        [FromServices] IAppDbContext dbContext)
    {
        var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        if(book is null)
           return NotFound();

        book.IsDeleted = true;
        book.ViewsCount = 0;
        book.ModifiedAt = DateTime.Now;

        await dbContext.SaveChangesAsync();
        
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBooks(
        [FromQuery] List<Guid> ids,
        [FromServices] IAppDbContext dbContext)
    {
        var books = await dbContext.Books
            .Where(b => ids.Contains(b.Id) && !b.IsDeleted)
            .ToListAsync();

        if(books.Count == 0)
            return NotFound("Books are not found.");

        foreach(var book in books)
        {
            book.IsDeleted = true;
            book.ModifiedAt = DateTime.Now;
        }

        await dbContext.SaveChangesAsync();

        return Ok(new { Message = $"{books.Count} books were deleted."});
    }
}