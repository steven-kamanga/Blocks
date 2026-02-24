using System.Net.Http.Headers;

namespace WebApp.Services.Api;

public abstract class BaseApiService
{
    protected readonly HttpClient Http;
    private readonly TokenStore _tokenStore;

    protected BaseApiService(HttpClient http, TokenStore tokenStore)
    {
        Http = http;
        _tokenStore = tokenStore;
    }

    protected void AddAuthHeader()
    {
        if (!string.IsNullOrEmpty(_tokenStore.Token))
        {
            Http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _tokenStore.Token);
        }
    }
}
