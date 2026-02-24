using Microsoft.AspNetCore.Identity;

namespace API.Models.Domain;

public class ApplicationUser : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
