namespace API.Models.Dto.Blocks;

public class SiteResponse
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Location { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? CreatedByUserId { get; set; }

}