using API.Models.Dto;
using API.Models.Dto.RawMaterial;

namespace API.Services.Interfaces;

public interface IRawMaterialService
{
    Task<RawMaterialCreateResponse> CreateAsync(RawMaterialCreateRequest request);
    Task<PaginatedResponse<RawMaterialResponse>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<RawMaterialResponse?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, RawMaterialUpdateRequest request);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateStockAsync(int id, decimal quantity);
}
