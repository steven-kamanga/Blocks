namespace API.Models.Domain;

public class Site
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public string? CreatedByUserId { get; set; }
    public ApplicationUser? CreatedBy { get; set; }
    
    // Machines at this site
    public ICollection<Machine> Machines { get; set; } = new List<Machine>();
}