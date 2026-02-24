namespace API.Models.Dto.RawMaterial;

public class RawMaterialCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = "kg";
    public decimal StockQuantity { get; set; } = 0;
    public decimal UnitCost { get; set; } = 0;
}
