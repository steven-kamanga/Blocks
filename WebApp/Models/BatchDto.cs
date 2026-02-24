namespace WebApp.Models;

public class BatchDto
{
    public int Id { get; set; }
    public string? BatchName { get; set; }
    public long? Quantity { get; set; }
    
    // Mix Ratio
    public int CementRatio { get; set; }
    public int SandRatio { get; set; }
    public int AggregateRatio { get; set; }
    
    // Materials used
    public decimal CementUsed { get; set; }
    public decimal SandUsed { get; set; }
    public decimal AggregateUsed { get; set; }
    public decimal WaterUsed { get; set; }
    
    // Machine
    public int? MachineId { get; set; }
    public string? MachineName { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? CreatedByUserId { get; set; }
}

public class BatchCreateRequest
{
    public string? BatchName { get; set; }
    public long Quantity { get; set; }
    
    // Mix Ratio
    public int CementRatio { get; set; } = 1;
    public int SandRatio { get; set; } = 2;
    public int AggregateRatio { get; set; } = 4;
    
    // Materials used
    public decimal CementUsed { get; set; }
    public decimal SandUsed { get; set; }
    public decimal AggregateUsed { get; set; }
    public decimal WaterUsed { get; set; }
    
    // Machine
    public int? MachineId { get; set; }
}

public class BatchCreateResponse
{
    public string? Message { get; set; }
    public BatchDto? Batch { get; set; }
}

public class BatchUpdateRequest
{
    public string? BatchName { get; set; }
    public long Quantity { get; set; }
    
    // Mix Ratio
    public int CementRatio { get; set; } = 1;
    public int SandRatio { get; set; } = 2;
    public int AggregateRatio { get; set; } = 4;
    
    // Materials used
    public decimal CementUsed { get; set; }
    public decimal SandUsed { get; set; }
    public decimal AggregateUsed { get; set; }
    public decimal WaterUsed { get; set; }
    
    // Machine
    public int? MachineId { get; set; }
}

public class BatchCalculationRequest
{
    public int CementRatio { get; set; } = 1;
    public int SandRatio { get; set; } = 2;
    public int AggregateRatio { get; set; } = 4;
    public decimal CementUsed { get; set; }
    public decimal SandUsed { get; set; }
    public decimal AggregateUsed { get; set; }
    public int? MachineId { get; set; }
}

public class BatchCalculationResponse
{
    public long EstimatedBlocks { get; set; }
    public int BlocksPerCycle { get; set; }
    public int EstimatedCycles { get; set; }
    public decimal TotalMixWeight { get; set; }
    public decimal WeightPerBlock { get; set; }
    public bool HasSufficientCement { get; set; }
    public bool HasSufficientSand { get; set; }
    public bool HasSufficientAggregate { get; set; }
    public decimal AvailableCement { get; set; }
    public decimal AvailableSand { get; set; }
    public decimal AvailableAggregate { get; set; }
    public string? Message { get; set; }
    public List<string> Warnings { get; set; } = new();
}
