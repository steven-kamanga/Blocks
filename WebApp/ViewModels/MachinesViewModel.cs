using WebApp.Models;
using WebApp.Services.Api;

namespace WebApp.ViewModels;

public class MachinesViewModel
{
    private readonly IMachineService _machineService;
    private readonly ISiteService _siteService;

    public MachinesViewModel(IMachineService machineService, ISiteService siteService)
    {
        _machineService = machineService;
        _siteService = siteService;
    }

    // State
    public List<MachineDto> Machines { get; private set; } = [];
    public List<SiteDto> Sites { get; private set; } = [];
    public MachineCreateRequest NewMachine { get; set; } = new();
    public MachineUpdateRequest EditRequest { get; set; } = new();
    public MachineDto? EditMachine { get; set; }
    public MachineDto? MachineToDelete { get; set; }

    public bool IsLoading { get; private set; } = true;
    public bool ShowCreateModal { get; set; }
    public bool ShowEditModal { get; set; }
    public bool ShowDeleteModal { get; set; }
    public bool IsSaving { get; private set; }
    public string? ErrorMessage { get; set; }

    // Pagination
    public int CurrentPage { get; private set; } = 1;
    public int PageSize { get; private set; } = 10;
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }
    public bool HasPreviousPage { get; private set; }
    public bool HasNextPage { get; private set; }

    // Actions
    public async Task LoadAsync()
    {
        IsLoading = true;
        var result = await _machineService.GetAllAsync(CurrentPage, PageSize);
        Machines = result.Items;
        TotalCount = result.TotalCount;
        TotalPages = result.TotalPages;
        HasPreviousPage = result.HasPreviousPage;
        HasNextPage = result.HasNextPage;
        var siteResult = await _siteService.GetAllAsync(1, 1000);
        Sites = siteResult.Items;
        IsLoading = false;
    }

    public async Task GoToPageAsync(int page)
    {
        if (page < 1 || page > TotalPages) return;
        CurrentPage = page;
        await LoadAsync();
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
        NewMachine = new MachineCreateRequest();
        ErrorMessage = null;
        ShowCreateModal = true;
    }

    public void CloseCreate()
    {
        ShowCreateModal = false;
        ErrorMessage = null;
    }

    public async Task<bool> CreateAsync()
    {
        if (string.IsNullOrWhiteSpace(NewMachine.Name))
        {
            ErrorMessage = "Machine name is required.";
            return false;
        }

        if (NewMachine.SiteId <= 0)
        {
            ErrorMessage = "Please select a site.";
            return false;
        }

        IsSaving = true;
        ErrorMessage = null;

        var result = await _machineService.CreateAsync(NewMachine);

        if (result != null)
        {
            CloseCreate();
            IsSaving = false;
            await LoadAsync();
            return true;
        }

        ErrorMessage = "Failed to create machine. Please try again.";
        IsSaving = false;
        return false;
    }

    public async Task<bool> ToggleStatusAsync(MachineDto machine)
    {
        var success = await _machineService.ToggleActiveAsync(machine.Id);
        if (success) machine.IsActive = !machine.IsActive;
        return success;
    }

    public void OpenEdit(MachineDto machine)
    {
        EditMachine = machine;
        EditRequest = new MachineUpdateRequest
        {
            Name = machine.Name,
            BlockSize = machine.BlockSize,
            BlocksPerBatch = machine.BlocksPerBatch,
            IsActive = machine.IsActive,
            SiteId = machine.SiteId
        };
        ErrorMessage = null;
        ShowEditModal = true;
    }

    public void CloseEdit()
    {
        ShowEditModal = false;
        EditMachine = null;
        ErrorMessage = null;
    }

    public async Task<bool> UpdateAsync()
    {
        if (EditMachine == null) return false;

        if (string.IsNullOrWhiteSpace(EditRequest.Name))
        {
            ErrorMessage = "Machine name is required.";
            return false;
        }

        IsSaving = true;
        ErrorMessage = null;

        var success = await _machineService.UpdateAsync(EditMachine.Id, EditRequest);

        if (success)
        {
            EditMachine.Name = EditRequest.Name;
            EditMachine.BlockSize = EditRequest.BlockSize;
            EditMachine.BlocksPerBatch = EditRequest.BlocksPerBatch;
            EditMachine.IsActive = EditRequest.IsActive;
            CloseEdit();
            IsSaving = false;
            return true;
        }

        ErrorMessage = "Failed to update machine. Please try again.";
        IsSaving = false;
        return false;
    }

    public void OpenDelete(MachineDto machine)
    {
        MachineToDelete = machine;
        ShowDeleteModal = true;
    }

    public void CloseDelete()
    {
        ShowDeleteModal = false;
        MachineToDelete = null;
    }

    public async Task<bool> DeleteAsync()
    {
        if (MachineToDelete == null) return false;

        IsSaving = true;
        var success = await _machineService.DeleteAsync(MachineToDelete.Id);

        if (success)
        {
            CloseDelete();
            await LoadAsync();
        }

        IsSaving = false;
        return success;
    }
}
