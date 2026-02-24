namespace WebApp.Models;

public class MachineDto
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

public class MachineCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public int BlocksPerBatch { get; set; } = 6;
    public string BlockSize { get; set; } = "6x8x16";
    public int SiteId { get; set; }
}

public class MachineUpdateRequest
{
    public string Name { get; set; } = string.Empty;
    public int BlocksPerBatch { get; set; } = 6;
    public string BlockSize { get; set; } = "6x8x16";
    public bool IsActive { get; set; } = true;
    public int SiteId { get; set; }
}
