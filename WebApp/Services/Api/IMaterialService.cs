using WebApp.Models;

namespace WebApp.Services.Api;

public interface IMaterialService
{
    Task<PaginatedResponse<RawMaterialDto>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<List<RawMaterialDto>> GetAllUnpagedAsync();
    Task<RawMaterialDto?> GetByIdAsync(int id);
    Task<RawMaterialDto?> CreateAsync(RawMaterialCreateRequest request);
    Task<bool> UpdateAsync(int id, RawMaterialUpdateRequest request);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateStockAsync(int id, decimal quantity);
}
