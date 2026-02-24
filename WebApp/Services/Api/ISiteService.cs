using WebApp.Models;

namespace WebApp.Services.Api;

public interface ISiteService
{
    Task<PaginatedResponse<SiteDto>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<SiteDto?> GetByIdAsync(int id);
    Task<SiteDto?> CreateAsync(SiteCreateRequest request);
    Task<bool> UpdateAsync(int id, SiteUpdateRequest request);
    Task<bool> DeleteAsync(int id);
}
