using API.Data;
using API.Models.Domain;
using API.Models.Dto;
using API.Models.Dto.RawMaterial;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Impl;

public class RawMaterialService(AppDbContext context) : IRawMaterialService
{
    public async Task<RawMaterialCreateResponse> CreateAsync(RawMaterialCreateRequest request)
    {
        var existing = await context.RawMaterials.FirstOrDefaultAsync(r => r.Name == request.Name);
        if (existing != null)
        {
            return new RawMaterialCreateResponse
            {
                Message = "A material with this name already exists.",
                Name = existing.Name,
                CreatedAt = existing.CreatedAt
            };
        }

        var material = new RawMaterial
        {
            Name = request.Name,
            Unit = request.Unit,
            StockQuantity = request.StockQuantity,
            UnitCost = request.UnitCost,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.RawMaterials.Add(material);
        await context.SaveChangesAsync();

        return new RawMaterialCreateResponse
        {
            Message = "Material created successfully!",
            Name = material.Name,
            CreatedAt = material.CreatedAt
        };
    }

    public async Task<PaginatedResponse<RawMaterialResponse>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        var query = context.RawMaterials.AsQueryable();
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RawMaterialResponse
            {
                Id = r.Id,
                Name = r.Name,
                Unit = r.Unit,
                StockQuantity = r.StockQuantity,
                UnitCost = r.UnitCost,
                CreatedAt = r.CreatedAt
            }).ToListAsync();

        return new PaginatedResponse<RawMaterialResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<RawMaterialResponse?> GetByIdAsync(int id)
    {
        var material = await context.RawMaterials.FindAsync(id);
        if (material == null) return null;

        return new RawMaterialResponse
        {
            Id = material.Id,
            Name = material.Name,
            Unit = material.Unit,
            StockQuantity = material.StockQuantity,
            UnitCost = material.UnitCost,
            CreatedAt = material.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(int id, RawMaterialUpdateRequest request)
    {
        var material = await context.RawMaterials.FindAsync(id);
        if (material == null) return false;

        if (material.Name != request.Name)
        {
            var existing = await context.RawMaterials.FirstOrDefaultAsync(m => m.Name == request.Name && m.Id != id);
            if (existing != null) return false;
        }

        material.Name = request.Name;
        material.Unit = request.Unit;
        material.StockQuantity = request.StockQuantity;
        material.UnitCost = request.UnitCost;
        material.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var material = await context.RawMaterials.FindAsync(id);
        if (material == null) return false;

        context.RawMaterials.Remove(material);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStockAsync(int id, decimal quantity)
    {
        var material = await context.RawMaterials.FindAsync(id);
        if (material == null) return false;

        material.StockQuantity = quantity;
        material.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return true;
    }
}
