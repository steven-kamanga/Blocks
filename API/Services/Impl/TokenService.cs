using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services.Impl;

public class TokenService : ITokenService
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(IConfiguration config)
    {
        _key = config["JwtSettings:Key"] ?? "super-secret-key-that-is-at-least-32-characters-long!!";
        _issuer = config["JwtSettings:Issuer"] ?? "BlocksApi";
        _audience = config["JwtSettings:Audience"] ?? "BlocksClient";
    }

    public string GenerateToken(string userId, string username, string? email)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, username),
            new Claim("unique_name", username),
            new Claim(ClaimTypes.Role, "User")
        };
        
        if (!string.IsNullOrEmpty(email))
        {
            claims.Add(new Claim(ClaimTypes.Email, email));
            claims.Add(new Claim("email", email));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims.ToArray(),
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
