using WebApp.Models;
using WebApp.Services.Api;

namespace WebApp.ViewModels;

public class BatchCalculatorViewModel
{
    private readonly IBatchService _batchService;
    private readonly IMachineService _machineService;
    private readonly IMaterialService _materialService;

    public BatchCalculatorViewModel(
        IBatchService batchService,
        IMachineService machineService,
        IMaterialService materialService)
    {
        _batchService = batchService;
        _machineService = machineService;
        _materialService = materialService;
    }

    // State
    public List<BatchDto> Batches { get; private set; } = [];
    public List<MachineDto> Machines { get; private set; } = [];
    public List<RawMaterialDto> Materials { get; private set; } = [];
    public BatchCreateRequest NewBatch { get; set; } = new();
    public BatchUpdateRequest EditRequest { get; set; } = new();
    public BatchCalculationResponse? Calculation { get; set; }
    public BatchDto? SelectedBatch { get; set; }
    public BatchDto? EditBatch { get; set; }
    public BatchDto? BatchToDelete { get; set; }

    public bool IsLoading { get; private set; } = true;
    public bool ShowCreateModal { get; set; }
    public bool ShowViewModal { get; set; }
    public bool ShowEditModal { get; set; }
    public bool ShowDownloadModal { get; set; }
    public bool ShowDeleteModal { get; set; }
    public bool IsSaving { get; private set; }
    public bool IsCalculating { get; private set; }
    public string? ErrorMessage { get; set; }
    public int CurrentStep { get; set; } = 1;

    // Pagination
    public int CurrentPage { get; private set; } = 1;
    public int PageSize { get; private set; } = 10;
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }
    public bool HasPreviousPage { get; private set; }
    public bool HasNextPage { get; private set; }

    // Helpers
    public RawMaterialDto? GetMaterial(string name) =>
        Materials.FirstOrDefault(m => m.Name == name);

    // Actions
    public async Task LoadAsync()
    {
        IsLoading = true;
        var result = await _batchService.GetAllAsync(CurrentPage, PageSize);
        Batches = result.Items;
        TotalCount = result.TotalCount;
        TotalPages = result.TotalPages;
        HasPreviousPage = result.HasPreviousPage;
        HasNextPage = result.HasNextPage;
        Machines = await _machineService.GetAllUnpagedAsync();
        Materials = await _materialService.GetAllUnpagedAsync();
        IsLoading = false;
    }

    public async Task GoToPageAsync(int page)
    {
        if (page < 1 || page > TotalPages) return;
        CurrentPage = page;
        var result = await _batchService.GetAllAsync(CurrentPage, PageSize);
        Batches = result.Items;
        TotalCount = result.TotalCount;
        TotalPages = result.TotalPages;
        HasPreviousPage = result.HasPreviousPage;
        HasNextPage = result.HasNextPage;
    }

    public async Task NextPageAsync()
    {
        if (HasNextPage) await GoToPageAsync(CurrentPage + 1);
    }

    public async Task PreviousPageAsync()
    {
        if (HasPreviousPage) await GoToPageAsync(CurrentPage - 1);
    }

    public void OpenCreate()
    {
        NewBatch = new BatchCreateRequest
        {
            CementRatio = 1,
            SandRatio = 2,
            AggregateRatio = 4
        };
        Calculation = null;
        ErrorMessage = null;
        CurrentStep = 1;
        ShowCreateModal = true;
    }

    public void CloseCreate()
    {
        ShowCreateModal = false;
        ErrorMessage = null;
        Calculation = null;
        CurrentStep = 1;
    }

    public void OpenDownload()
    {
        ShowDownloadModal = true;
    }

    public void CloseDownload()
    {
        ShowDownloadModal = false;
        SelectedBatch = null;
    }

    public async Task<bool> NextStepAsync()
    {
        ErrorMessage = null;

        if (CurrentStep == 1 && string.IsNullOrWhiteSpace(NewBatch.BatchName))
        {
            ErrorMessage = "Batch name is required.";
            return false;
        }

        if (CurrentStep < 4)
        {
            CurrentStep++;
            if (CurrentStep == 4)
            {
                await CalculateOutputAsync();
            }
        }
        return true;
    }

    public void PrevStep()
    {
        ErrorMessage = null;
        if (CurrentStep > 1) CurrentStep--;
    }

    public void SetRatio(int cement, int sand, int aggregate)
    {
        NewBatch.CementRatio = cement;
        NewBatch.SandRatio = sand;
        NewBatch.AggregateRatio = aggregate;
    }

    public async Task CalculateOutputAsync()
    {
        IsCalculating = true;
        var request = new BatchCalculationRequest
        {
            CementRatio = NewBatch.CementRatio,
            SandRatio = NewBatch.SandRatio,
            AggregateRatio = NewBatch.AggregateRatio,
            CementUsed = NewBatch.CementUsed,
            SandUsed = NewBatch.SandUsed,
            AggregateUsed = NewBatch.AggregateUsed,
            MachineId = NewBatch.MachineId
        };

        Calculation = await _batchService.CalculateOutputAsync(request);

        if (NewBatch.Quantity == 0 && Calculation != null)
        {
            NewBatch.Quantity = Calculation.EstimatedBlocks;
        }

        IsCalculating = false;
    }

    public async Task OnMachineChangedAsync(int? machineId)
    {
        NewBatch.MachineId = machineId;
        await CalculateOutputAsync();
    }

    public async Task<bool> CreateAsync()
    {
        if (string.IsNullOrWhiteSpace(NewBatch.BatchName))
        {
            ErrorMessage = "Batch name is required.";
            return false;
        }

        IsSaving = true;
        ErrorMessage = null;

        var result = await _batchService.CreateAsync(NewBatch);

        if (result?.Message?.Contains("successfully") == true)
        {
            await LoadAsync();
            CloseCreate();
            IsSaving = false;
            return true;
        }

        ErrorMessage = result?.Message ?? "Failed to create batch. Please try again.";
        IsSaving = false;
        return false;
    }

    public void View(BatchDto batch)
    {
        SelectedBatch = batch;
        ShowViewModal = true;
    }

    public void CloseView()
    {
        ShowViewModal = false;
        SelectedBatch = null;
    }

    public void OpenEdit(BatchDto batch)
    {
        EditBatch = batch;
        EditRequest = new BatchUpdateRequest
        {
            BatchName = batch.BatchName,
            Quantity = batch.Quantity ?? 0,
            CementRatio = batch.CementRatio,
            SandRatio = batch.SandRatio,
            AggregateRatio = batch.AggregateRatio,
            CementUsed = batch.CementUsed,
            SandUsed = batch.SandUsed,
            AggregateUsed = batch.AggregateUsed,
            WaterUsed = batch.WaterUsed,
            MachineId = batch.MachineId
        };
        ErrorMessage = null;
        ShowEditModal = true;
    }

    public void CloseEdit()
    {
        ShowEditModal = false;
        EditBatch = null;
        ErrorMessage = null;
    }

    public async Task<bool> UpdateAsync()
    {
        if (EditBatch == null) return false;

        if (string.IsNullOrWhiteSpace(EditRequest.BatchName))
        {
            ErrorMessage = "Batch name is required.";
            return false;
        }

        IsSaving = true;
        ErrorMessage = null;

        var success = await _batchService.UpdateAsync(EditBatch.Id, EditRequest);

        if (success)
        {
            await LoadAsync();
            CloseEdit();
            IsSaving = false;
            return true;
        }

        ErrorMessage = "Failed to update batch. Please try again.";
        IsSaving = false;
        return false;
    }

    public void OpenDelete(BatchDto batch)
    {
        BatchToDelete = batch;
        ShowDeleteModal = true;
    }

    public void CloseDelete()
    {
        ShowDeleteModal = false;
        BatchToDelete = null;
    }

    public async Task<bool> DeleteAsync()
    {
        if (BatchToDelete == null) return false;

        IsSaving = true;
        var success = await _batchService.DeleteAsync(BatchToDelete.Id);

        if (success)
        {
            CloseDelete();
            await GoToPageAsync(CurrentPage);
        }

        IsSaving = false;
        return success;
    }
}
