namespace API.Models.Dto.Machine;

public class MachineResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BlocksPerBatch { get; set; }
    public string BlockSize { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int SiteId { get; set; }
    public string SiteName { get; set; } = string.Empty;
}
