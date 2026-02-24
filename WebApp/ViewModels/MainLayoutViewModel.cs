using WebApp.Services;
using WebApp.Services.Api;

namespace WebApp.ViewModels;

public class MainLayoutViewModel
{
    private readonly IUserService _userService;
    private readonly CustomAuthStateProvider _authStateProvider;

    public MainLayoutViewModel(IUserService userService, CustomAuthStateProvider authStateProvider)
    {
        _userService = userService;
        _authStateProvider = authStateProvider;
    }

    public string? Username { get; private set; }
    public string? Email { get; private set; }

    public async Task LoadUserInfoAsync()
    {
        try
        {
            var userInfo = await _userService.GetCurrentUserAsync();
            if (userInfo != null)
            {
                Username = userInfo.Username;
                Email = userInfo.Email;
                return;
            }
        }
        catch { }

        // Fallback to JWT claims
        try
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            if (authState.User?.Identity?.IsAuthenticated == true)
            {
                Username = authState.User.Identity.Name ??
                           authState.User.FindFirst("unique_name")?.Value;
                Email = authState.User.FindFirst("email")?.Value ??
                        authState.User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            }
        }
        catch { }
    }
}
