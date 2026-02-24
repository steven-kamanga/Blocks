using API.Models.Dto;
using API.Models.Dto.Machine;

namespace API.Services.Interfaces;

public interface IMachineService
{
    Task<MachineCreateResponse> CreateAsync(MachineCreateRequest request);
    Task<PaginatedResponse<MachineResponse>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<MachineResponse?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, MachineUpdateRequest request);
    Task<bool> DeleteAsync(int id);
    Task<bool> ToggleActiveAsync(int id);
}
