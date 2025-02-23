namespace BookManagement.Models.DTOS;

public class AddBookDto
{
    public string? Title { get; set; }
    public int PublicationYear { get; set; }
    public string? Author { get; set; }
}