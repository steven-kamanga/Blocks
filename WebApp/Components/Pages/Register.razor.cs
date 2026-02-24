using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WebApp.Models.Auth;
using WebApp.Services;

namespace WebApp.Components.Pages;

public partial class Register
{
    [Inject]
    private IAuthService AuthService { get; set; } = default!;
    
    [Inject]
    private NavigationManager Navigation { get; set; } = default!;
    
    [Inject]
    private CustomAuthStateProvider AuthStateProvider { get; set; } = default!;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    private RegisterRequest _registerModel = new();
    private string _confirmPassword = string.Empty;
    private string? _errorMessage;
    private string? _successMessage;
    private bool _isLoading;

    private async Task HandleRegister()
    {
        _isLoading = true;
        _errorMessage = null;
        _successMessage = null;

        try
        {
            if (string.IsNullOrWhiteSpace(_registerModel.Username) ||
                string.IsNullOrWhiteSpace(_registerModel.Email) ||
                string.IsNullOrWhiteSpace(_registerModel.Password))
            {
                _errorMessage = "Please fill in all fields.";
                return;
            }

            if (_registerModel.Password.Length < 6)
            {
                _errorMessage = "Password must be at least 6 characters.";
                return;
            }

            if (_registerModel.Password != _confirmPassword)
            {
                _errorMessage = "Passwords do not match.";
                return;
            }

            var result = await AuthService.RegisterAsync(_registerModel);

            if (result.Success)
            {
                await JS.InvokeVoidAsync("eval", "document.cookie='auth_session=1;path=/;max-age=604800;SameSite=Strict'");
                
                AuthStateProvider.NotifyAuthenticationStateChanged();
                Navigation.NavigateTo("/", forceLoad: true);
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
