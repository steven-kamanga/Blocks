namespace API.Models.Dto.RawMaterial;

public class RawMaterialCreateResponse
{
    public string Message { get; set; } = string.Empty;
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
}
