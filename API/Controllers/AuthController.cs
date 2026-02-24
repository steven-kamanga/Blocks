using System.Security.Claims;
using API.Models.Dto.Auth;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || 
            string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new AuthResponse 
            { 
                Success = false, 
                Message = "Please fill in all fields!" 
            });
        }
        
        if (request.Password.Length < 6)
        {
            return BadRequest(new AuthResponse 
            { 
                Success = false, 
                Message = "Password must be at least 6 characters!" 
            });
        }
        
        var result = await _authService.RegisterAsync(request);
        
        if (result.Success)
            return Ok(result);
        else
            return BadRequest(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || 
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new AuthResponse 
            { 
                Success = false, 
                Message = "Please enter username and password!" 
            });
        }
        
        var result = await _authService.LoginAsync(request);
        
        if (result.Success)
            return Ok(result);
        else
            return BadRequest(result);
    }
    
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var username = User.Identity?.Name;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        
        return Ok(new 
        { 
            UserId = userId,
            Username = username,
            Email = email
        });
    }
}
