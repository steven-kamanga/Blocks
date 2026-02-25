using Microsoft.AspNetCore.Components;
using WebApp.Models;

namespace WebApp.Components.Partials.Materials;

public partial class _CreateMaterialsModal : ComponentBase
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public string ErrorMessage { get; set; } = string.Empty;
    [Parameter] public RawMaterialCreateRequest NewMaterial { get; set; } = new RawMaterialCreateRequest();
    [Parameter] public bool Saving { get; set; }
    [Parameter] public EventCallback CreateMaterial { get; set; }
}