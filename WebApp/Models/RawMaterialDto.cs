namespace WebApp.Models;

public class RawMaterialDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal StockQuantity { get; set; }
    public decimal UnitCost { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RawMaterialCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = "kg";
    public decimal StockQuantity { get; set; }
    public decimal UnitCost { get; set; }
}

public class RawMaterialUpdateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = "kg";
    public decimal StockQuantity { get; set; }
    public decimal UnitCost { get; set; }
}
