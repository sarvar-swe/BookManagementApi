namespace BookManagement.Models.DTOS;

public class GetBookWithPopularityScoreDto
{
    public GetBookWithPopularityScoreDto(Book model)
    {
        Title = model.Title;
        PopularityScore = (model.ViewsCount * 0.5) + ((DateTime.Now.Year - model.PublicationYear) * 2);
    }
    public string? Title { get; set; }
    public double PopularityScore { get; set; }
}