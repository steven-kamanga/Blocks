using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WebApp.Models.Auth;
using WebApp.Services;

namespace WebApp.Components.Pages;

public partial class Login
{
    [Inject]
    private IAuthService AuthService { get; set; } = default!;
    
    [Inject]
    private NavigationManager Navigation { get; set; } = default!;
    
    [Inject]
    private CustomAuthStateProvider AuthStateProvider { get; set; } = default!;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    [SupplyParameterFromQuery]
    public string? ReturnUrl { get; set; }

    private LoginRequest _loginModel = new();
    private string? _errorMessage;
    private bool _isLoading;

    private async Task HandleLogin()
    {
        _isLoading = true;
        _errorMessage = null;

        try
        {
            if (string.IsNullOrWhiteSpace(_loginModel.Username) || string.IsNullOrWhiteSpace(_loginModel.Password))
            {
                _errorMessage = "Please enter username and password.";
                return;
            }

            var result = await AuthService.LoginAsync(_loginModel);

            if (result.Success)
            {
                // Set auth cookie for middleware redirect
                await JS.InvokeVoidAsync("eval", "document.cookie='auth_session=1;path=/;max-age=604800;SameSite=Strict'");
                
                AuthStateProvider.NotifyAuthenticationStateChanged();
                
                // Parse returnUrl and ensure it's a relative path
                var navigateTo = "/";
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    var decodedUrl = Uri.UnescapeDataString(ReturnUrl);
                    if (Uri.TryCreate(decodedUrl, UriKind.Absolute, out var absoluteUri))
                    {
                        navigateTo = absoluteUri.PathAndQuery;
                    }
                    else
                    {
                        navigateTo = decodedUrl;
                    }
                }
                
                Navigation.NavigateTo(navigateTo, forceLoad: true);
            }
            else
            {
                _errorMessage = result.Message;
            }
        }
        catch (Exception)
        {
            _errorMessage = "An error occurred. Please try again.";
        }
        finally
        {
            _isLoading = false;
        }
    }
}
