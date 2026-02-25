using Microsoft.AspNetCore.Components;
using WebApp.Models;

namespace WebApp.Components.Partials.Machines;

public partial class _DeleteMachineModal : ComponentBase
{    [Parameter] public bool Show { get; set; }
    [Parameter] public MachineDto MachineToDelete { get; set; } = new MachineDto();
    [Parameter] public bool IsSaving { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback OnDelete { get; set; }
}