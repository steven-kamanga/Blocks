using API.Models.Domain;
using API.Models.Dto.Auth;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace API.Services.Impl;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByNameAsync(request.Username);
        if (existingUser != null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Username already taken! Try another one."
            };
        }

        var existingEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingEmail != null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Email already registered! Maybe try logging in?"
            };
        }

        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse
            {
                Success = false,
                Message = $"Registration failed: {errors}"
            };
        }

        var token = _tokenService.GenerateToken(user.Id, user.UserName!, user.Email);

        return new AuthResponse
        {
            Success = true,
            Message = "Registration successful! Welcome aboard!",
            Token = token,
            Username = user.UserName,
            Email = user.Email
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username ?? "");

        if (user == null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "User not found! Maybe try registering?"
            };
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password ?? "");

        if (!isPasswordValid)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Wrong password! Try again."
            };
        }

        var token = _tokenService.GenerateToken(user.Id, user.UserName!, user.Email);

        return new AuthResponse
        {
            Success = true,
            Message = "Welcome back!",
            Token = token,
            Username = user.UserName,
            Email = user.Email
        };
    }
}

