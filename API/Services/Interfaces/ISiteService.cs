using API.Models.Dto;
using API.Models.Dto.Blocks;
using API.Models.Dto.Site;

namespace API.Services.Interfaces;

public interface ISiteService
{
    Task<SiteCreateResponse> CreateSiteAsync(SiteCreateRequest request, string userId);
    Task<PaginatedResponse<SiteResponse>> GetAllSitesAsync(int page = 1, int pageSize = 10);
    Task<SiteResponse?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, SiteUpdateRequest request);
    Task<bool> DeleteAsync(int id);
}