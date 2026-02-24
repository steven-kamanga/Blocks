using WebApp.Models;
using WebApp.Services.Api;

namespace WebApp.ViewModels;

public class SitesViewModel
{
    private readonly ISiteService _siteService;

    public SitesViewModel(ISiteService siteService)
    {
        _siteService = siteService;
    }

    // State
    public List<SiteDto> Sites { get; private set; } = [];
    public SiteCreateRequest NewSite { get; set; } = new();
    public SiteUpdateRequest EditRequest { get; set; } = new();
    public SiteDto? SelectedSite { get; set; }
    public SiteDto? EditSite { get; set; }
    public SiteDto? SiteToDelete { get; set; }

    public bool IsLoading { get; private set; } = true;
    public bool ShowCreateModal { get; set; }
    public bool ShowViewModal { get; set; }
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

    public async Task LoadAsync()
    {
        IsLoading = true;
        var result = await _siteService.GetAllAsync(CurrentPage, PageSize);
        Sites = result.Items;
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
        NewSite = new SiteCreateRequest();
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
        if (string.IsNullOrWhiteSpace(NewSite.Name) || string.IsNullOrWhiteSpace(NewSite.Location))
        {
            ErrorMessage = "Name and Location are required.";
            return false;
        }

        IsSaving = true;
        ErrorMessage = null;

        var result = await _siteService.CreateAsync(NewSite);

        if (result != null)
        {
            CloseCreate();
            IsSaving = false;
            await LoadAsync();
            return true;
        }

        ErrorMessage = "Failed to create site. Please try again.";
        IsSaving = false;
        return false;
    }

    public void View(SiteDto site)
    {
        SelectedSite = site;
        ShowViewModal = true;
    }

    public void CloseView()
    {
        ShowViewModal = false;
        SelectedSite = null;
    }

    public void OpenEdit(SiteDto site)
    {
        EditSite = site;
        EditRequest = new SiteUpdateRequest
        {
            Name = site.Name,
            Location = site.Location
        };
        ErrorMessage = null;
        ShowEditModal = true;
    }

    public void CloseEdit()
    {
        ShowEditModal = false;
        EditSite = null;
        ErrorMessage = null;
    }

    public async Task<bool> UpdateAsync()
    {
        if (EditSite == null) return false;

        if (string.IsNullOrWhiteSpace(EditRequest.Name) || string.IsNullOrWhiteSpace(EditRequest.Location))
        {
            ErrorMessage = "Name and Location are required.";
            return false;
        }

        IsSaving = true;
        ErrorMessage = null;

        var success = await _siteService.UpdateAsync(EditSite.Id, EditRequest);

        if (success)
        {
            EditSite.Name = EditRequest.Name;
            EditSite.Location = EditRequest.Location;
            CloseEdit();
            IsSaving = false;
            return true;
        }

        ErrorMessage = "Failed to update site. Please try again.";
        IsSaving = false;
        return false;
    }

    public void OpenDelete(SiteDto site)
    {
        SiteToDelete = site;
        ShowDeleteModal = true;
    }

    public void CloseDelete()
    {
        ShowDeleteModal = false;
        SiteToDelete = null;
    }

    public async Task<bool> DeleteAsync()
    {
        if (SiteToDelete == null) return false;

        IsSaving = true;
        var success = await _siteService.DeleteAsync(SiteToDelete.Id);

        if (success)
        {
            CloseDelete();
            await LoadAsync();
        }

        IsSaving = false;
        return success;
    }
}
