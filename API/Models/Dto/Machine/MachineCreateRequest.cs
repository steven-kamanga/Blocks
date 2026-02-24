namespace API.Models.Dto.Machine;

public class MachineCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public int BlocksPerBatch { get; set; } = 6;
    public string BlockSize { get; set; } = "6x8x16";
    public int SiteId { get; set; }
}
