using Microsoft.AspNetCore.Components;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Components.Pages;

public partial class Sites : ComponentBase
{
    [Inject] private SitesViewModel VM { get; set; } = default!;
    [Inject] private CustomAuthStateProvider AuthStateProvider { get; set; } = default!;

    // Expose VM state (razor stays unchanged)
    private List<SiteDto> _sites => VM.Sites;
    private bool _isLoading => VM.IsLoading;
    private bool _isSaving => VM.IsSaving;
    private string? _errorMessage => VM.ErrorMessage;
    private SiteCreateRequest _newSite => VM.NewSite;
    private SiteUpdateRequest _editRequest => VM.EditRequest;
    private SiteDto? _selectedSite => VM.SelectedSite;
    private SiteDto? _editSite => VM.EditSite;
    private SiteDto? _siteToDelete => VM.SiteToDelete;
    private bool _showCreateModal => VM.ShowCreateModal;
    private bool _showViewModal => VM.ShowViewModal;
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
    private async Task CreateSite() => await VM.CreateAsync();
    private void ViewSite(SiteDto site) => VM.View(site);
    private void CloseViewModal() => VM.CloseView();
    private void OpenEditModal(SiteDto site) => VM.OpenEdit(site);
    private void CloseEditModal() => VM.CloseEdit();
    private async Task UpdateSite() => await VM.UpdateAsync();
    private void OpenDeleteModal(SiteDto site) => VM.OpenDelete(site);
    private void CloseDeleteModal() => VM.CloseDelete();
    private async Task DeleteSite() => await VM.DeleteAsync();
    private async Task GoToPage(int page) { await VM.GoToPageAsync(page); StateHasChanged(); }
    private async Task NextPage() { await VM.NextPageAsync(); StateHasChanged(); }
    private async Task PreviousPage() { await VM.PreviousPageAsync(); StateHasChanged(); }
}