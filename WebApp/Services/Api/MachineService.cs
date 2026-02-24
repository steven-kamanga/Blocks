using System.Net.Http.Json;
using WebApp.Models;

namespace WebApp.Services.Api;

public class MachineService : BaseApiService, IMachineService
{
    private readonly ILogger<MachineService> _logger;

    public MachineService(HttpClient http, TokenStore tokenStore, ILogger<MachineService> logger)
        : base(http, tokenStore)
    {
        _logger = logger;
    }

    public async Task<PaginatedResponse<MachineDto>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            AddAuthHeader();
            return await Http.GetFromJsonAsync<PaginatedResponse<MachineDto>>($"api/v1/machines?page={page}&pageSize={pageSize}")
                   ?? new PaginatedResponse<MachineDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching machines");
            return new PaginatedResponse<MachineDto>();
        }
    }

    public async Task<List<MachineDto>> GetAllUnpagedAsync()
    {
        try
        {
            AddAuthHeader();
            var result = await Http.GetFromJsonAsync<PaginatedResponse<MachineDto>>("api/v1/machines?page=1&pageSize=1000");
            return result?.Items ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all machines");
            return [];
        }
    }

    public async Task<MachineDto?> GetByIdAsync(int id)
    {
        try
        {
            AddAuthHeader();
            return await Http.GetFromJsonAsync<MachineDto>($"api/v1/machines/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching machine {Id}", id);
            return null;
        }
    }

    public async Task<MachineDto?> CreateAsync(MachineCreateRequest request)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.PostAsJsonAsync("api/v1/machines", request);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<MachineDto>()
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating machine");
            return null;
        }
    }

    public async Task<bool> UpdateAsync(int id, MachineUpdateRequest request)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.PutAsJsonAsync($"api/v1/machines/{id}", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating machine {Id}", id);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.DeleteAsync($"api/v1/machines/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting machine {Id}", id);
            return false;
        }
    }

    public async Task<bool> ToggleActiveAsync(int id)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.PatchAsync($"api/v1/machines/{id}/toggle", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling machine status");
            return false;
        }
    }
}
