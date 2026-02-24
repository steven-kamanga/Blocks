using System.Text;
using API.Data;
using API.Models.Domain;
using API.Services.Impl;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBatchService, BatchService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IRawMaterialService, RawMaterialService>();
builder.Services.AddScoped<IMachineService, MachineService>();

var jwtKey = builder.Configuration["JwtSettings:Key"] ?? "super-secret-goose-key-that-is-definitely-32-chars-long!!";
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "BlocksApi";
var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? "BlocksClient";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync(); 
    
    if (!await db.RawMaterials.AnyAsync())
    {
        db.RawMaterials.AddRange(
            new RawMaterial { Name = "Cement", Unit = "kg", StockQuantity = 0, UnitCost = 0 },
            new RawMaterial { Name = "Sand", Unit = "kg", StockQuantity = 0, UnitCost = 0 },
            new RawMaterial { Name = "Aggregate", Unit = "kg", StockQuantity = 0, UnitCost = 0 },
            new RawMaterial { Name = "Water", Unit = "liters", StockQuantity = 0, UnitCost = 0 }
        );
        await db.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
