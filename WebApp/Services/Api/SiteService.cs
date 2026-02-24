using System.Net.Http.Json;
using WebApp.Models;

namespace WebApp.Services.Api;

public class SiteService : BaseApiService, ISiteService
{
    private readonly ILogger<SiteService> _logger;

    public SiteService(HttpClient http, TokenStore tokenStore, ILogger<SiteService> logger)
        : base(http, tokenStore)
    {
        _logger = logger;
    }

    public async Task<PaginatedResponse<SiteDto>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            AddAuthHeader();
            return await Http.GetFromJsonAsync<PaginatedResponse<SiteDto>>($"api/v1/site?page={page}&pageSize={pageSize}")
                   ?? new PaginatedResponse<SiteDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching sites");
            return new PaginatedResponse<SiteDto>();
        }
    }

    public async Task<SiteDto?> GetByIdAsync(int id)
    {
        try
        {
            AddAuthHeader();
            return await Http.GetFromJsonAsync<SiteDto>($"api/v1/site/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching site {Id}", id);
            return null;
        }
    }

    public async Task<SiteDto?> CreateAsync(SiteCreateRequest request)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.PostAsJsonAsync("api/v1/site", request);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<SiteDto>()
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating site");
            return null;
        }
    }

    public async Task<bool> UpdateAsync(int id, SiteUpdateRequest request)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.PutAsJsonAsync($"api/v1/site/{id}", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating site {Id}", id);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.DeleteAsync($"api/v1/site/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting site {Id}", id);
            return false;
        }
    }
}
