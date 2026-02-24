using WebApp.Models;
using WebApp.Services.Api;

namespace WebApp.ViewModels;

public class HomeViewModel
{
    private readonly ISiteService _siteService;
    private readonly IBatchService _batchService;

    public HomeViewModel(ISiteService siteService, IBatchService batchService)
    {
        _siteService = siteService;
        _batchService = batchService;
    }

    public int SiteCount { get; private set; }
    public int BatchCount { get; private set; }
    public long TotalBlocks { get; private set; }

    public async Task LoadAsync()
    {
        var sites = await _siteService.GetAllAsync(1, 1);
        var batches = await _batchService.GetAllAsync(1, 1000);

        SiteCount = sites.TotalCount;
        BatchCount = batches.TotalCount;
        TotalBlocks = batches.Items.Sum(b => b.Quantity ?? 0);
    }
}
