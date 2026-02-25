using Microsoft.AspNetCore.Components;
using WebApp.Models;

namespace WebApp.Components.Partials.Machines;

public partial class _CreateMachineModal : ComponentBase
{
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public bool Show { get; set; }
    [Parameter] public string ErrorMessage { get; set; } = string.Empty;
    [Parameter] public MachineCreateRequest NewMachine { get; set; } = new MachineCreateRequest();
    [Parameter] public bool Saving { get; set; }
    [Parameter] public EventCallback CreateMachine { get; set; }
    [Parameter] public List<SiteDto> Sites { get; set; } = new List<SiteDto>();
}