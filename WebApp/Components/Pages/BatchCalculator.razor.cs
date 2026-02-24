using Microsoft.AspNetCore.Components;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Components.Pages;

public partial class BatchCalculator : ComponentBase
{
    [Inject] private BatchCalculatorViewModel VM { get; set; } = default!;
    [Inject] private CustomAuthStateProvider AuthStateProvider { get; set; } = default!;

    private List<BatchDto> _batches => VM.Batches;
    public List<MachineDto> _machines => VM.Machines;
    public List<RawMaterialDto> _materials => VM.Materials;
    private bool _isLoading => VM.IsLoading;
    public bool _isSaving => VM.IsSaving;
    public bool _isCalculating => VM.IsCalculating;
    public string? _errorMessage => VM.ErrorMessage;
    public int _currentStep => VM.CurrentStep;
    public BatchCreateRequest _newBatch => VM.NewBatch;
    public BatchUpdateRequest _editRequest => VM.EditRequest;
    public BatchCalculationResponse? _calculation => VM.Calculation;
    public BatchDto? _selectedBatch => VM.SelectedBatch;
    public BatchDto? _editBatch => VM.EditBatch;
    public BatchDto? _batchToDelete => VM.BatchToDelete;
    public bool _showCreateModal => VM.ShowCreateModal;
    public bool _showViewModal => VM.ShowViewModal;
    public bool _showEditModal => VM.ShowEditModal;
    public bool _showDeleteModal => VM.ShowDeleteModal;

    private int _currentPage => VM.CurrentPage;
    private int _totalPages => VM.TotalPages;
    private int _totalCount => VM.TotalCount;
    private bool _hasPreviousPage => VM.HasPreviousPage;
    private bool _hasNextPage => VM.HasNextPage;

    private HashSet<int> _selectedBatchIds = new();
    private bool _isAllSelected => _batches.Count > 0 && _batches.All(b => _selectedBatchIds.Contains(b.Id));

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
    public void CloseCreateModal() => VM.CloseCreate();
    public void SetRatio(int c, int s, int a) => VM.SetRatio(c, s, a);
    public RawMaterialDto? GetMaterial(string name) => VM.GetMaterial(name);
    public async Task NextStep() => await VM.NextStepAsync();
    public void PrevStep() => VM.PrevStep();
    public async Task CreateBatch() => await VM.CreateAsync();
    private void ViewBatch(BatchDto batch) => VM.View(batch);
    public void CloseViewModal() => VM.CloseView();
    public void CloseDownloadModal() => VM.CloseDownload();
    private void OpenEditModal(BatchDto batch) => VM.OpenEdit(batch);
    public void CloseEditModal() => VM.CloseEdit();
    public async Task UpdateBatch() => await VM.UpdateAsync();
    private void OpenDeleteModal(BatchDto batch) => VM.OpenDelete(batch);
    public void CloseDeleteModal() => VM.CloseDelete();
    public async Task DeleteBatch() => await VM.DeleteAsync();
    private async Task GoToPage(int page) { await VM.GoToPageAsync(page); StateHasChanged(); }
    private async Task NextPage() { await VM.NextPageAsync(); StateHasChanged(); }
    private async Task PreviousPage() { await VM.PreviousPageAsync(); StateHasChanged(); }

    private void ToggleSelection(int batchId)
    {
        if (_selectedBatchIds.Contains(batchId))
            _selectedBatchIds.Remove(batchId);
        else
            _selectedBatchIds.Add(batchId);
    }

    private void ToggleSelectAll()
    {
        if (_isAllSelected)
            _selectedBatchIds.Clear();
        else
            _selectedBatchIds = _batches.Select(b => b.Id).ToHashSet();
    }

    private void ClearSelection() => _selectedBatchIds.Clear();

    public async Task OnMachineChanged(ChangeEventArgs e)
    {
        int? machineId = int.TryParse(e.Value?.ToString(), out var id) ? id : null;
        await VM.OnMachineChangedAsync(machineId);
    }
}