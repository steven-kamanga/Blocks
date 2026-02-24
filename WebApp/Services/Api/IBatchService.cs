using WebApp.Models;

namespace WebApp.Services.Api;

public interface IBatchService
{
    Task<PaginatedResponse<BatchDto>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<BatchDto?> GetByIdAsync(int id);
    Task<BatchCreateResponse?> CreateAsync(BatchCreateRequest request);
    Task<bool> UpdateAsync(int id, BatchUpdateRequest request);
    Task<bool> DeleteAsync(int id);
    Task<BatchCalculationResponse?> CalculateOutputAsync(BatchCalculationRequest request);
}
