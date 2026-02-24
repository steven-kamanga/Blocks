namespace API.Models.Domain;

public class RawMaterial
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Cement, Sand, Aggregate, Water
    public string Unit { get; set; } = "kg"; // kg, liters, bags
    public decimal StockQuantity { get; set; } = 0;
    public decimal UnitCost { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
