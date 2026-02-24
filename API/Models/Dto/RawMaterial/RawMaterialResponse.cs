namespace API.Models.Dto.RawMaterial;

public class RawMaterialResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal StockQuantity { get; set; }
    public decimal UnitCost { get; set; }
    public DateTime CreatedAt { get; set; }
}
