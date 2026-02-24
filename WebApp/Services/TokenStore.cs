namespace WebApp.Services;

public class TokenStore
{
    public string? Token { get; set; }
    
    public void SetToken(string? token) => Token = token;
    
    public void ClearToken() => Token = null;
}
