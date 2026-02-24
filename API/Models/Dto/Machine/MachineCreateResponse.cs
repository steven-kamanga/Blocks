namespace API.Models.Dto.Machine;

public class MachineCreateResponse
{
    public string Message { get; set; } = string.Empty;
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
}
