namespace API.Models.Dto.Batch;

public class BatchResponse
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
