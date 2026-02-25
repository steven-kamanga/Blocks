using Microsoft.AspNetCore.Components;
using WebApp.Models;

namespace WebApp.Components.Partials.Materials;

public partial class _DeleteMaterialsModal : ComponentBase
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public RawMaterialDto MaterialToDelete { get; set; } = new RawMaterialDto();
    [Parameter] public bool IsSaving { get; set; }
    [Parameter] public EventCallback DeleteMaterial { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
}