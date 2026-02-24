using API.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<BatchItem> BatchItems => Set<BatchItem>();
    public DbSet<Site> Sites => Set<Site>();
    public DbSet<RawMaterial> RawMaterials => Set<RawMaterial>();
    public DbSet<Machine> Machines => Set<Machine>();
}
