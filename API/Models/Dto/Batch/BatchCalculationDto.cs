namespace API.Models.Dto.Batch;

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
