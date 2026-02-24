namespace API.Models.Domain;

public class BatchItem
{
    public int Id { get; set; }
    public string? BatchName { get; set; }
    public long? Quantity { get; set; } = 0; // Total blocks produced
    
    // Mix Ratio (e.g., 1:2:4 = Cement:Sand:Aggregate)
    public int CementRatio { get; set; } = 1;
    public int SandRatio { get; set; } = 2;
    public int AggregateRatio { get; set; } = 4;
    
    // Materials used (in kg or bags)
    public decimal CementUsed { get; set; } = 0;
    public decimal SandUsed { get; set; } = 0;
    public decimal AggregateUsed { get; set; } = 0;
    public decimal WaterUsed { get; set; } = 0; // liters
    
    // Machine used
    public int? MachineId { get; set; }
    public Machine? Machine { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }

    public string? CreatedByUserId { get; set; }
    public ApplicationUser? CreatedBy { get; set; }
}
