namespace API.Models.Domain;

public class Machine
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g., "Block Press A"
    public int BlocksPerBatch { get; set; } = 6; // typical output per mold cycle
    public string BlockSize { get; set; } = "6x8x16"; // inches (width x height x length)
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Site relationship
    public int SiteId { get; set; }
    public Site Site { get; set; } = null!;
}
