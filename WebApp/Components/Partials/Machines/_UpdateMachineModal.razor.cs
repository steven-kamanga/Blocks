using Microsoft.AspNetCore.Components;
using WebApp.Models;

namespace WebApp.Components.Partials.Machines;

public partial class _UpdateMachineModal : ComponentBase
{
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public bool Show { get; set; }
    [Parameter] public string ErrorMessage { get; set; } = string.Empty;
    [Parameter] public MachineUpdateRequest EditRequest { get; set; } = new MachineUpdateRequest();
    [Parameter] public bool IsSaving { get; set; }
    [Parameter] public EventCallback UpdateMachine { get; set; }
    [Parameter] public List<SiteDto> Sites { get; set; } = new List<SiteDto>();
}