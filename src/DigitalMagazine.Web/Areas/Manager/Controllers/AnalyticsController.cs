using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Piranha.Manager.Controllers;
using DigitalMagazine.Application.Analytics;

namespace DigitalMagazine.Web.Areas.Manager.Controllers;

[Area("Manager")]
[Route("manager/analytics")]
[Authorize(Policy = Piranha.Manager.Permission.Admin)]
public class AnalyticsController : Controller
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [Route("")]
    public async Task<IActionResult> Index()
    {
        // Get top 20 articles
        var stats = await _analyticsService.GetTopArticlesAsync(20);
        return View(stats);
    }
}
