namespace API.Models.Dto.Batch;

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
