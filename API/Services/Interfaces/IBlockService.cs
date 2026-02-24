using API.Models.Dto;
using API.Models.Dto.Batch;

namespace API.Services.Interfaces;

public interface IBatchService
{
    Task<BatchCreateResponse> CreateBatchAsync(BatchCreateRequest request, string userId);
    Task<PaginatedResponse<BatchResponse>> GetAllBatchesAsync(int page = 1, int pageSize = 10);
    Task<BatchResponse?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, BatchUpdateRequest request);
    Task<bool> DeleteAsync(int id);
    Task<BatchCalculationResponse> CalculateOutputAsync(BatchCalculationRequest request);
}