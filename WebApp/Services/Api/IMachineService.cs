using WebApp.Models;

namespace WebApp.Services.Api;

public interface IMachineService
{
    Task<PaginatedResponse<MachineDto>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<List<MachineDto>> GetAllUnpagedAsync();
    Task<MachineDto?> GetByIdAsync(int id);
    Task<MachineDto?> CreateAsync(MachineCreateRequest request);
    Task<bool> UpdateAsync(int id, MachineUpdateRequest request);
    Task<bool> DeleteAsync(int id);
    Task<bool> ToggleActiveAsync(int id);
}
