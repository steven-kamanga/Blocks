using API.Data;
using API.Models.Domain;
using API.Models.Dto;
using API.Models.Dto.Batch;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Impl;

public class BatchService(AppDbContext context) : IBatchService
{
    private const decimal StandardBlockWeight = 16m;

    public async Task<BatchCreateResponse> CreateBatchAsync(BatchCreateRequest request, string userId)
    {
        var existingBatch = await context.BatchItems.FirstOrDefaultAsync(b => b.BatchName == request.BatchName);
        if (existingBatch != null)
        {
            return new BatchCreateResponse
            {
                Message = "A batch with this name already exists.",
                BatchName = existingBatch.BatchName,
                CreatedAt = existingBatch.CreatedAt
            };
        }

        var cement = await context.RawMaterials.FirstOrDefaultAsync(m => m.Name == "Cement");
        var sand = await context.RawMaterials.FirstOrDefaultAsync(m => m.Name == "Sand");
        var aggregate = await context.RawMaterials.FirstOrDefaultAsync(m => m.Name == "Aggregate");
        var water = await context.RawMaterials.FirstOrDefaultAsync(m => m.Name == "Water");

        var cementKg = request.CementUsed * 50; 
        
        if (cement != null && cement.StockQuantity < request.CementUsed)
        {
            return new BatchCreateResponse
            {
                Message = $"Insufficient cement. Available: {cement.StockQuantity} bags, Required: {request.CementUsed} bags"
            };
        }
        if (sand != null && sand.StockQuantity < request.SandUsed)
        {
            return new BatchCreateResponse
            {
                Message = $"Insufficient sand. Available: {sand.StockQuantity} kg, Required: {request.SandUsed} kg"
            };
        }
        if (aggregate != null && aggregate.StockQuantity < request.AggregateUsed)
        {
            return new BatchCreateResponse
            {
                Message = $"Insufficient aggregate. Available: {aggregate.StockQuantity} kg, Required: {request.AggregateUsed} kg"
            };
        }

        if (cement != null)
        {
            cement.StockQuantity -= request.CementUsed;
            cement.UpdatedAt = DateTime.UtcNow;
        }
        if (sand != null)
        {
            sand.StockQuantity -= request.SandUsed;
            sand.UpdatedAt = DateTime.UtcNow;
        }
        if (aggregate != null)
        {
            aggregate.StockQuantity -= request.AggregateUsed;
            aggregate.UpdatedAt = DateTime.UtcNow;
        }
        if (water != null && water.StockQuantity >= request.WaterUsed)
        {
            water.StockQuantity -= request.WaterUsed;
            water.UpdatedAt = DateTime.UtcNow;
        }

        var batchItem = new BatchItem
        {
            BatchName = request.BatchName,
            Quantity = request.Quantity,
            CementRatio = request.CementRatio,
            SandRatio = request.SandRatio,
            AggregateRatio = request.AggregateRatio,
            CementUsed = request.CementUsed,
            SandUsed = request.SandUsed,
            AggregateUsed = request.AggregateUsed,
            WaterUsed = request.WaterUsed,
            MachineId = request.MachineId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedByUserId = userId
        };

        context.BatchItems.Add(batchItem);
        await context.SaveChangesAsync();

        return new BatchCreateResponse
        {
            Message = "Batch created successfully!",
            BatchName = batchItem.BatchName,
            CreatedAt = batchItem.CreatedAt
        };
    }

    public async Task<PaginatedResponse<BatchResponse>> GetAllBatchesAsync(int page = 1, int pageSize = 10)
    {
        var query = context.BatchItems.Include(b => b.Machine).AsQueryable();
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BatchResponse
            {
                Id = b.Id,
                BatchName = b.BatchName,
                Quantity = b.Quantity ?? 0,
                CementRatio = b.CementRatio,
                SandRatio = b.SandRatio,
                AggregateRatio = b.AggregateRatio,
                CementUsed = b.CementUsed,
                SandUsed = b.SandUsed,
                AggregateUsed = b.AggregateUsed,
                WaterUsed = b.WaterUsed,
                MachineId = b.MachineId,
                MachineName = b.Machine != null ? b.Machine.Name : null,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
                CreatedByUserId = b.CreatedByUserId
            }).ToListAsync();

        return new PaginatedResponse<BatchResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<BatchResponse?> GetByIdAsync(int id)
    {
        var batch = await context.BatchItems
            .Include(b => b.Machine)
            .FirstOrDefaultAsync(b => b.Id == id);
        
        if (batch == null) return null;

        return new BatchResponse
        {
            Id = batch.Id,
            BatchName = batch.BatchName,
            Quantity = batch.Quantity ?? 0,
            CementRatio = batch.CementRatio,
            SandRatio = batch.SandRatio,
            AggregateRatio = batch.AggregateRatio,
            CementUsed = batch.CementUsed,
            SandUsed = batch.SandUsed,
            AggregateUsed = batch.AggregateUsed,
            WaterUsed = batch.WaterUsed,
            MachineId = batch.MachineId,
            MachineName = batch.Machine?.Name,
            CreatedAt = batch.CreatedAt,
            UpdatedAt = batch.UpdatedAt,
            CreatedByUserId = batch.CreatedByUserId
        };
    }

    public async Task<bool> UpdateAsync(int id, BatchUpdateRequest request)
    {
        var batch = await context.BatchItems.FindAsync(id);
        if (batch == null) return false;

        if (batch.BatchName != request.BatchName)
        {
            var existing = await context.BatchItems.FirstOrDefaultAsync(b => b.BatchName == request.BatchName && b.Id != id);
            if (existing != null) return false;
        }

        batch.BatchName = request.BatchName;
        batch.Quantity = request.Quantity;
        batch.CementRatio = request.CementRatio;
        batch.SandRatio = request.SandRatio;
        batch.AggregateRatio = request.AggregateRatio;
        batch.CementUsed = request.CementUsed;
        batch.SandUsed = request.SandUsed;
        batch.AggregateUsed = request.AggregateUsed;
        batch.WaterUsed = request.WaterUsed;
        batch.MachineId = request.MachineId;
        batch.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var batch = await context.BatchItems.FindAsync(id);
        if (batch == null) return false;

        context.BatchItems.Remove(batch);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<BatchCalculationResponse> CalculateOutputAsync(BatchCalculationRequest request)
    {
        var cement = await context.RawMaterials.FirstOrDefaultAsync(m => m.Name == "Cement");
        var sand = await context.RawMaterials.FirstOrDefaultAsync(m => m.Name == "Sand");
        var aggregate = await context.RawMaterials.FirstOrDefaultAsync(m => m.Name == "Aggregate");

        var response = new BatchCalculationResponse
        {
            AvailableCement = cement?.StockQuantity ?? 0,
            AvailableSand = sand?.StockQuantity ?? 0,
            AvailableAggregate = aggregate?.StockQuantity ?? 0,
            HasSufficientCement = cement != null && cement.StockQuantity >= request.CementUsed,
            HasSufficientSand = sand != null && sand.StockQuantity >= request.SandUsed,
            HasSufficientAggregate = aggregate != null && aggregate.StockQuantity >= request.AggregateUsed
        };

        var cementKg = request.CementUsed * 50;
        response.TotalMixWeight = cementKg + request.SandUsed + request.AggregateUsed;
        
        response.WeightPerBlock = StandardBlockWeight;
        response.EstimatedBlocks = (long)(response.TotalMixWeight / StandardBlockWeight);

        if (request.MachineId.HasValue)
        {
            var machine = await context.Machines.FindAsync(request.MachineId.Value);
            if (machine != null)
            {
                response.BlocksPerCycle = machine.BlocksPerBatch;
                response.EstimatedCycles = response.EstimatedBlocks > 0 
                    ? (int)Math.Ceiling((decimal)response.EstimatedBlocks / machine.BlocksPerBatch)
                    : 0;
            }
        }

        if (!response.HasSufficientCement)
        {
            response.Warnings.Add($"Insufficient Cement: need {request.CementUsed:N1} kg, have {response.AvailableCement:N1} kg");
        }
        if (!response.HasSufficientSand)
        {
            response.Warnings.Add($"Insufficient Sand: need {request.SandUsed:N1} kg, have {response.AvailableSand:N1} kg");
        }
        if (!response.HasSufficientAggregate)
        {
            response.Warnings.Add($"Insufficient Aggregate: need {request.AggregateUsed:N1} kg, have {response.AvailableAggregate:N1} kg");
        }

        if (response.Warnings.Count > 0)
        {
            response.Message = "Warning: Insufficient materials in stock";
        }
        else
        {
            response.Message = "Sufficient materials available";
        }

        return response;
    }
}