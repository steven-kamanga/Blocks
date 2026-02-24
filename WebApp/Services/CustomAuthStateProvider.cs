using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;

namespace WebApp.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _localStorage;
    private readonly TokenStore _tokenStore;
    private readonly ILogger<CustomAuthStateProvider> _logger;
    
    private const string TokenKey = "authToken";
    
    // Cache the auth state to handle prerendering
    private AuthenticationState? _cachedAuthState;
    private bool _isInitialized;
    
    public CustomAuthStateProvider(
        ProtectedLocalStorage localStorage,
        TokenStore tokenStore,
        ILogger<CustomAuthStateProvider> logger)
    {
        _localStorage = localStorage;
        _tokenStore = tokenStore;
        _logger = logger;
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Return cached state if we're in prerendering phase
        if (_cachedAuthState != null)
        {
            return _cachedAuthState;
        }
        
        try
        {
            var tokenResult = await _localStorage.GetAsync<string>(TokenKey);
            
            if (!tokenResult.Success || string.IsNullOrEmpty(tokenResult.Value))
            {
                _cachedAuthState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                _isInitialized = true;
                return _cachedAuthState;
            }
            
            var token = tokenResult.Value;
            
            // Store token in memory for HTTP handlers
            _tokenStore.SetToken(token);
            
            // Parse the JWT to extract claims
            var claims = ParseClaimsFromJwt(token);
            
            if (claims == null || !claims.Any())
            {
                _cachedAuthState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                _isInitialized = true;
                return _cachedAuthState;
            }
            
            // Check if token is expired
            var expClaim = claims.FirstOrDefault(c => c.Type == "exp");
            if (expClaim != null && long.TryParse(expClaim.Value, out var exp))
            {
                var expDate = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                if (expDate < DateTime.UtcNow)
                {
                    // Token expired, clear it
                    await _localStorage.DeleteAsync(TokenKey);
                    _tokenStore.ClearToken();
                    _cachedAuthState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                    _isInitialized = true;
                    return _cachedAuthState;
                }
            }
            
            var identity = new ClaimsIdentity(claims, "jwt", "unique_name", System.Security.Claims.ClaimTypes.Role);
            var user = new ClaimsPrincipal(identity);
            
            _cachedAuthState = new AuthenticationState(user);
            _isInitialized = true;
            return _cachedAuthState;
        }
        catch (InvalidOperationException)
        {
            // This happens during prerendering - return anonymous but don't cache
            // to allow proper check once interactive
            _logger.LogDebug("ProtectedLocalStorage not available (prerendering)");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting authentication state");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
    
    public bool IsInitialized => _isInitialized;
    
    public void NotifyAuthenticationStateChanged()
    {
        _cachedAuthState = null; // Clear cache to force refresh
        _isInitialized = false;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    
    private static IEnumerable<Claim>? ParseClaimsFromJwt(string jwt)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }
        catch
        {
            return null;
        }
    }
}
