namespace BookManagement.Models.DTOS;

public class GetBookDto
{
    public GetBookDto(Book model)
    {
        Id = model.Id;
        Title = model.Title;
        PublicationYear = model.PublicationYear;
        Author = model.Author;
        ViewsCount = model.ViewsCount;
        CreatedAt = model.CreatedAt;
        ModifiedAt = model.ModifiedAt;
    }

    public Guid Id { get; set; }
    public string? Title { get; set; }
    public int PublicationYear { get; set; }
    public string? Author { get; set; }
    public int ViewsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}