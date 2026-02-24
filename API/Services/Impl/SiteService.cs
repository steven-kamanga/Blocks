using API.Data;
using API.Models.Domain;
using API.Models.Dto;
using API.Models.Dto.Blocks;
using API.Models.Dto.Site;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Impl;

public class SiteService(AppDbContext context) : ISiteService
{
    public Task<SiteCreateResponse> CreateSiteAsync(SiteCreateRequest request, string userId)
    {
        var site = new Site
        {
            Name = request.Name,
            Location = request.Location,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Sites.Add(site);
        context.SaveChanges();
        var response = new SiteCreateResponse
        {
            Id = site.Id,
            Name = site.Name,
            Location = site.Location,
            CreatedAt = site.CreatedAt,
            UpdatedAt = site.UpdatedAt,
            CreatedByUserId = site.CreatedByUserId
        };
        return Task.FromResult(response);
    }

    public async Task<PaginatedResponse<SiteResponse>> GetAllSitesAsync(int page = 1, int pageSize = 10)
    {
        var query = context.Sites.AsQueryable();
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new SiteResponse
            {
                Id = s.Id,
                Name = s.Name,
                Location = s.Location,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                CreatedByUserId = s.CreatedByUserId
            }).ToListAsync();

        return new PaginatedResponse<SiteResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<SiteResponse?> GetByIdAsync(int id)
    {
        var site = await context.Sites.FindAsync(id);
        if (site == null) return null;

        return new SiteResponse
        {
            Id = site.Id,
            Name = site.Name,
            Location = site.Location,
            CreatedAt = site.CreatedAt,
            UpdatedAt = site.UpdatedAt,
            CreatedByUserId = site.CreatedByUserId
        };
    }

    public async Task<bool> UpdateAsync(int id, SiteUpdateRequest request)
    {
        var site = await context.Sites.FindAsync(id);
        if (site == null) return false;

        site.Name = request.Name;
        site.Location = request.Location;
        site.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var site = await context.Sites.FindAsync(id);
        if (site == null) return false;

        context.Sites.Remove(site);
        await context.SaveChangesAsync();
        return true;
    }
}
