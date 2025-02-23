namespace BookManagement.Models.DTOS;

public class UpdateBookDto
{
    public string? Title { get; set; }
    public int PublicationYear { get; set; }
    public string? Author { get; set; }
}