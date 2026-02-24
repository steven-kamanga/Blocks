using API.Data;
using API.Models.Domain;
using API.Models.Dto;
using API.Models.Dto.Machine;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Impl;

public class MachineService(AppDbContext context) : IMachineService
{
    public async Task<MachineCreateResponse> CreateAsync(MachineCreateRequest request)
    {
        var existing = await context.Machines.FirstOrDefaultAsync(m => m.Name == request.Name);
        if (existing != null)
        {
            return new MachineCreateResponse
            {
                Message = "A machine with this name already exists.",
                Name = existing.Name,
                CreatedAt = existing.CreatedAt
            };
        }

        var machine = new Machine
        {
            Name = request.Name,
            BlocksPerBatch = request.BlocksPerBatch,
            BlockSize = request.BlockSize,
            SiteId = request.SiteId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Machines.Add(machine);
        await context.SaveChangesAsync();

        return new MachineCreateResponse
        {
            Message = "Machine created successfully!",
            Name = machine.Name,
            CreatedAt = machine.CreatedAt
        };
    }

    public async Task<PaginatedResponse<MachineResponse>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        var query = context.Machines.Include(m => m.Site).AsQueryable();
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MachineResponse
            {
                Id = m.Id,
                Name = m.Name,
                BlocksPerBatch = m.BlocksPerBatch,
                BlockSize = m.BlockSize,
                IsActive = m.IsActive,
                CreatedAt = m.CreatedAt,
                SiteId = m.SiteId,
                SiteName = m.Site.Name ?? string.Empty
            }).ToListAsync();

        return new PaginatedResponse<MachineResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<MachineResponse?> GetByIdAsync(int id)
    {
        var machine = await context.Machines
            .Include(m => m.Site)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (machine == null) return null;

        return new MachineResponse
        {
            Id = machine.Id,
            Name = machine.Name,
            BlocksPerBatch = machine.BlocksPerBatch,
            BlockSize = machine.BlockSize,
            IsActive = machine.IsActive,
            CreatedAt = machine.CreatedAt,
            SiteId = machine.SiteId,
            SiteName = machine.Site?.Name ?? string.Empty
        };
    }

    public async Task<bool> UpdateAsync(int id, MachineUpdateRequest request)
    {
        var machine = await context.Machines.FindAsync(id);
        if (machine == null) return false;

        // Check if name is being changed to an existing name
        if (machine.Name != request.Name)
        {
            var existing = await context.Machines.FirstOrDefaultAsync(m => m.Name == request.Name && m.Id != id);
            if (existing != null) return false;
        }

        machine.Name = request.Name;
        machine.BlocksPerBatch = request.BlocksPerBatch;
        machine.BlockSize = request.BlockSize;
        machine.IsActive = request.IsActive;
        machine.SiteId = request.SiteId;
        machine.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var machine = await context.Machines.FindAsync(id);
        if (machine == null) return false;

        context.Machines.Remove(machine);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleActiveAsync(int id)
    {
        var machine = await context.Machines.FindAsync(id);
        if (machine == null) return false;

        machine.IsActive = !machine.IsActive;
        machine.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return true;
    }
}
