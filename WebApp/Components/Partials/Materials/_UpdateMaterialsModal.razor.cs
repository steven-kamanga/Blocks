using Microsoft.AspNetCore.Components;
using WebApp.Models;

namespace WebApp.Components.Partials.Materials;

public partial class _UpdateMaterialsModal : ComponentBase
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public RawMaterialDto MaterialToEdit { get; set; } = new RawMaterialDto();
    [Parameter] public RawMaterialUpdateRequest EditRequest { get; set; } = new RawMaterialUpdateRequest();
    [Parameter] public bool IsSaving { get; set; }
    [Parameter] public string ErrorMessage { get; set; } = string.Empty;
    [Parameter] public EventCallback UpdateMaterial { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
}