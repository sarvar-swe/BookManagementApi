namespace BookManagement.Models;

public class Book
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public int PublicationYear { get; set; }
    public string? Author { get; set; }
    public int ViewsCount { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

}
