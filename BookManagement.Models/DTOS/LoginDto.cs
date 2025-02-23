namespace BookManagement.Models.DTOS;

public class LoginDto
{
    public string? UserName { get; set; }
    public string? AccessToken { get; set; }
    public int ExpiresIn { get; set; }
}