using System.Net.Http.Json;
using WebApp.Models;

namespace WebApp.Services.Api;

public class UserService : BaseApiService, IUserService
{
    private readonly ILogger<UserService> _logger;

    public UserService(HttpClient http, TokenStore tokenStore, ILogger<UserService> logger)
        : base(http, tokenStore)
    {
        _logger = logger;
    }

    public async Task<UserInfoDto?> GetCurrentUserAsync()
    {
        try
        {
            AddAuthHeader();
            return await Http.GetFromJsonAsync<UserInfoDto>("api/v1/auth/me");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching current user");
            return null;
        }
    }
}
