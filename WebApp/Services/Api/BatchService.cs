using System.Net.Http.Json;
using WebApp.Models;

namespace WebApp.Services.Api;

public class BatchService : BaseApiService, IBatchService
{
    private readonly ILogger<BatchService> _logger;

    public BatchService(HttpClient http, TokenStore tokenStore, ILogger<BatchService> logger)
        : base(http, tokenStore)
    {
        _logger = logger;
    }

    public async Task<PaginatedResponse<BatchDto>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            AddAuthHeader();
            return await Http.GetFromJsonAsync<PaginatedResponse<BatchDto>>($"api/v1/batch?page={page}&pageSize={pageSize}")
                   ?? new PaginatedResponse<BatchDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching batches");
            return new PaginatedResponse<BatchDto>();
        }
    }

    public async Task<BatchDto?> GetByIdAsync(int id)
    {
        try
        {
            AddAuthHeader();
            return await Http.GetFromJsonAsync<BatchDto>($"api/v1/batch/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching batch {Id}", id);
            return null;
        }
    }

    public async Task<BatchCreateResponse?> CreateAsync(BatchCreateRequest request)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.PostAsJsonAsync("api/v1/batch", request);
            return await response.Content.ReadFromJsonAsync<BatchCreateResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating batch");
            return null;
        }
    }

    public async Task<bool> UpdateAsync(int id, BatchUpdateRequest request)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.PutAsJsonAsync($"api/v1/batch/{id}", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating batch {Id}", id);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.DeleteAsync($"api/v1/batch/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting batch {Id}", id);
            return false;
        }
    }

    public async Task<BatchCalculationResponse?> CalculateOutputAsync(BatchCalculationRequest request)
    {
        try
        {
            AddAuthHeader();
            var response = await Http.PostAsJsonAsync("api/v1/batch/calculate", request);
            return await response.Content.ReadFromJsonAsync<BatchCalculationResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating batch output");
            return null;
        }
    }
}
