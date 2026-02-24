using WebApp.Models;
using WebApp.Services.Api;

namespace WebApp.ViewModels;

public class MaterialsViewModel
{
    private readonly IMaterialService _materialService;

    public MaterialsViewModel(IMaterialService materialService)
    {
        _materialService = materialService;
    }

    // State
    public List<RawMaterialDto> Materials { get; private set; } = [];
    public RawMaterialCreateRequest NewMaterial { get; set; } = new();
    public RawMaterialUpdateRequest EditRequest { get; set; } = new();
    public RawMaterialDto? SelectedMaterial { get; set; }
    public RawMaterialDto? EditMaterial { get; set; }
    public RawMaterialDto? MaterialToDelete { get; set; }
    public decimal NewStockQuantity { get; set; }

    public bool IsLoading { get; private set; } = true;
    public bool ShowCreateModal { get; set; }
    public bool ShowStockModal { get; set; }
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
        var result = await _materialService.GetAllAsync(CurrentPage, PageSize);
        Materials = result.Items;
        TotalCount = result.TotalCount;
        TotalPages = result.TotalPages;
        HasPreviousPage = result.HasPreviousPage;
        HasNextPage = result.HasNextPage;
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
        NewMaterial = new RawMaterialCreateRequest();
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
        if (string.IsNullOrWhiteSpace(NewMaterial.Name))
        {
            ErrorMessage = "Material name is required.";
            return false;
        }

        IsSaving = true;
        ErrorMessage = null;

        var result = await _materialService.CreateAsync(NewMaterial);

        if (result != null)
        {
            CloseCreate();
            IsSaving = false;
            await LoadAsync();
            return true;
        }

        ErrorMessage = "Failed to create material. Please try again.";
        IsSaving = false;
        return false;
    }

    public void OpenStock(RawMaterialDto material)
    {
        SelectedMaterial = material;
        NewStockQuantity = material.StockQuantity;
        ShowStockModal = true;
    }

    public void CloseStock()
    {
        ShowStockModal = false;
        SelectedMaterial = null;
    }

    public async Task<bool> UpdateStockAsync()
    {
        if (SelectedMaterial == null) return false;

        IsSaving = true;
        var success = await _materialService.UpdateStockAsync(SelectedMaterial.Id, NewStockQuantity);

        if (success)
        {
            SelectedMaterial.StockQuantity = NewStockQuantity;
            CloseStock();
        }

        IsSaving = false;
        return success;
    }

    public void OpenEdit(RawMaterialDto material)
    {
        EditMaterial = material;
        EditRequest = new RawMaterialUpdateRequest
        {
            Name = material.Name,
            Unit = material.Unit,
            StockQuantity = material.StockQuantity,
            UnitCost = material.UnitCost
        };
        ErrorMessage = null;
        ShowEditModal = true;
    }

    public void CloseEdit()
    {
        ShowEditModal = false;
        EditMaterial = null;
        ErrorMessage = null;
    }

    public async Task<bool> UpdateAsync()
    {
        if (EditMaterial == null) return false;

        if (string.IsNullOrWhiteSpace(EditRequest.Name))
        {
            ErrorMessage = "Material name is required.";
            return false;
        }

        IsSaving = true;
        ErrorMessage = null;

        var success = await _materialService.UpdateAsync(EditMaterial.Id, EditRequest);

        if (success)
        {
            EditMaterial.Name = EditRequest.Name;
            EditMaterial.Unit = EditRequest.Unit;
            EditMaterial.StockQuantity = EditRequest.StockQuantity;
            EditMaterial.UnitCost = EditRequest.UnitCost;
            CloseEdit();
            IsSaving = false;
            return true;
        }

        ErrorMessage = "Failed to update material. Please try again.";
        IsSaving = false;
        return false;
    }

    public void OpenDelete(RawMaterialDto material)
    {
        MaterialToDelete = material;
        ShowDeleteModal = true;
    }

    public void CloseDelete()
    {
        ShowDeleteModal = false;
        MaterialToDelete = null;
    }

    public async Task<bool> DeleteAsync()
    {
        if (MaterialToDelete == null) return false;

        IsSaving = true;
        var success = await _materialService.DeleteAsync(MaterialToDelete.Id);

        if (success)
        {
            CloseDelete();
            await LoadAsync();
        }

        IsSaving = false;
        return success;
    }
}
