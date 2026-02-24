namespace API.Models.Dto.RawMaterial;

public class RawMaterialUpdateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = "kg";
    public decimal StockQuantity { get; set; }
    public decimal UnitCost { get; set; }
}
