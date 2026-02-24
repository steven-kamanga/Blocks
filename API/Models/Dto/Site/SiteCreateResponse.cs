namespace API.Models.Dto.Site;

public class SiteCreateResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? CreatedByUserId { get; set; }

}