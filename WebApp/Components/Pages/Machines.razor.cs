using Microsoft.AspNetCore.Components;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Components.Pages;

public partial class Machines : ComponentBase
{
    [Inject] private MachinesViewModel VM { get; set; } = default!;
    [Inject] private CustomAuthStateProvider AuthStateProvider { get; set; } = default!;

    // Expose VM state (razor stays unchanged)
    private List<MachineDto> _machines => VM.Machines;
    private List<SiteDto> _sites => VM.Sites;
    private bool _isLoading => VM.IsLoading;
    private bool _isSaving => VM.IsSaving;
    private string? _errorMessage => VM.ErrorMessage;
    private MachineCreateRequest _newMachine => VM.NewMachine;
    private MachineUpdateRequest _editRequest => VM.EditRequest;
    private MachineDto? _editMachine => VM.EditMachine;
    private MachineDto? _machineToDelete => VM.MachineToDelete;
    private bool _showCreateModal => VM.ShowCreateModal;
    private bool _showEditModal => VM.ShowEditModal;
    private bool _showDeleteModal => VM.ShowDeleteModal;

    // Pagination
    private int _currentPage => VM.CurrentPage;
    private int _totalPages => VM.TotalPages;
    private int _totalCount => VM.TotalCount;
    private bool _hasPreviousPage => VM.HasPreviousPage;
    private bool _hasNextPage => VM.HasNextPage;

    private bool _dataLoaded;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !_dataLoaded)
        {
            await AuthStateProvider.GetAuthenticationStateAsync();
            await VM.LoadAsync();
            _dataLoaded = true;
            StateHasChanged();
        }
    }

    private void OpenCreateModal() => VM.OpenCreate();
    private void CloseCreateModal() => VM.CloseCreate();
    private async Task CreateMachine() => await VM.CreateAsync();
    private async Task ToggleStatus(MachineDto m) => await VM.ToggleStatusAsync(m);
    private void OpenEditModal(MachineDto m) => VM.OpenEdit(m);
    private void CloseEditModal() => VM.CloseEdit();
    private async Task UpdateMachine() => await VM.UpdateAsync();
    private void OpenDeleteModal(MachineDto m) => VM.OpenDelete(m);
    private void CloseDeleteModal() => VM.CloseDelete();
    private async Task DeleteMachine() => await VM.DeleteAsync();
    private async Task GoToPage(int page) { await VM.GoToPageAsync(page); StateHasChanged(); }
    private async Task NextPage() { await VM.NextPageAsync(); StateHasChanged(); }
    private async Task PreviousPage() { await VM.PreviousPageAsync(); StateHasChanged(); }
}
