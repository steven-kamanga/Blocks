using Microsoft.AspNetCore.Components;
using WebApp.Models;

namespace WebApp.Components.Partials.BatchCalculator;

public partial class _DeteleBatchModal : ComponentBase
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public BatchDto? BatchToDelete { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback OnDeleted { get; set; }
    [Parameter] public EventCallback<int> OnBatchDeleted { get; set; }
    private bool IsSaving = false;
}