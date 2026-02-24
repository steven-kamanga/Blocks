using System.Net.Http.Json;
using WebApp.Models;

namespace WebApp.Services.Api;

public class MaterialService : BaseApiService, IMaterialService
{
    private readonly ILogger<MaterialService> _logger;

    public MaterialService(HttpClient http, TokenStore tokenStore, ILogger<MaterialService> logger)
        : base(http, tokenStore)
    {
        _logger = logger;
    }

    public async Task<PaginatedResponse<RawMaterialDto>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            AddAuthHeader();
            return await Http.GetFromJsonAsync<PaginatedResponse<RawMaterialDto>>($"api/v1/materials?page={page}&pageSize={pageSize}")
                   ?? new PaginatedResponse<RawMaterialDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching materials");
            return new PaginatedResponse<RawMaterialDto>();
        }
    }

    public async Task<List<RawMaterialDto>> GetAllUnpagedAsync()
    {
        try
        {
            AddAuthHeader();
            var result = await Http.GetFromJsonAsync<PaginatedResponse<RawMaterialDto>>("api/v1/materials?page=1&pageSize=1000");
            return result?.Items ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all materials");
            return [];
        }
    }

    public async Task<RawMaterialDto?> GetByIdAsync(int id)
    {
        try
        {
            AddAuthHeader();
            return await Http.GetFromJsonAsync<RawMaterialDto>($"api/v1/materials/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching material {Id}", id);
            return null;
        }
    }

    public async Task<RawMaterialDto?> CreateAsync(RawMaterialCreateRequest request)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.PostAsJsonAsync("api/v1/materials", request);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<RawMaterialDto>()
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating material");
            return null;
        }
    }

    public async Task<bool> UpdateAsync(int id, RawMaterialUpdateRequest request)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.PutAsJsonAsync($"api/v1/materials/{id}", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating material {Id}", id);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.DeleteAsync($"api/v1/materials/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting material {Id}", id);
            return false;
        }
    }

    public async Task<bool> UpdateStockAsync(int id, decimal quantity)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.PatchAsJsonAsync($"api/v1/materials/{id}/stock", quantity);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating material stock");
            return false;
        }
    }
}
