using Microsoft.AspNetCore.Components;
using WebApp.Models;

namespace WebApp.Components.Partials.Materials;

public partial class _ShowMaterialsModal
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public RawMaterialDto SelectedMaterial { get; set; } = new RawMaterialDto();
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback UpdateStock { get; set; }
    [Parameter] public bool IsSaving { get; set; }
    [Parameter] public decimal NewStockQuantity { get; set; }
}