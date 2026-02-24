namespace API.Models.Dto.Auth;

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}