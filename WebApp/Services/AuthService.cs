using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using WebApp.Models.Auth;

namespace WebApp.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage _localStorage;
    private readonly TokenStore _tokenStore;
    private readonly ILogger<AuthService> _logger;
    
    private const string TokenKey = "authToken";
    private const string UsernameKey = "username";
    
    public AuthService(
        HttpClient httpClient,
        ProtectedLocalStorage localStorage,
        TokenStore tokenStore,
        ILogger<AuthService> logger)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _tokenStore = tokenStore;
        _logger = logger;
    }
    
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/auth/login", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            
            if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
            {
                await _localStorage.SetAsync(TokenKey, result.Token);
                await _localStorage.SetAsync(UsernameKey, result.Username ?? request.Username);
                _tokenStore.SetToken(result.Token);
                _logger.LogInformation("User {Username} logged in successfully", request.Username);
            }
            
            return result ?? new AuthResponse { Success = false, Message = "Unknown error occurred" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return new AuthResponse { Success = false, Message = "Login failed. Please try again." };
        }
    }
    
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/auth/register", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            
            if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
            {
                await _localStorage.SetAsync(TokenKey, result.Token);
                await _localStorage.SetAsync(UsernameKey, result.Username ?? request.Username);
                _tokenStore.SetToken(result.Token);
                _logger.LogInformation("User {Username} registered successfully", request.Username);
            }
            
            return result ?? new AuthResponse { Success = false, Message = "Unknown error occurred" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return new AuthResponse { Success = false, Message = "Registration failed. Please try again." };
        }
    }
    
    public async Task LogoutAsync()
    {
        try
        {
            await _localStorage.DeleteAsync(TokenKey);
            await _localStorage.DeleteAsync(UsernameKey);
            _tokenStore.ClearToken();
            _logger.LogInformation("User logged out");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
        }
    }
    
    public async Task<string?> GetTokenAsync()
    {
        try
        {
            var result = await _localStorage.GetAsync<string>(TokenKey);
            return result.Success ? result.Value : null;
        }
        catch (Exception)
        {
            // This can happen during prerendering
            return null;
        }
    }
    
    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }
}
