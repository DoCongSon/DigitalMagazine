using Microsoft.AspNetCore.Mvc;
using DigitalMagazine.Application.Home;
using DigitalMagazine.Web.Models.Home;

namespace DigitalMagazine.Web.Controllers;

public class HomeController : Controller
{
    private readonly IHomePageService _homePageService;

    public HomeController(IHomePageService homePageService)
        => _homePageService = homePageService;

    public async Task<IActionResult> Index()
    {
        var dto = await _homePageService.GetHomePageAsync();
        
        var viewModel = new HomeViewModel
        {
            PageTitle = dto?.PageTitle ?? "DigitalMagazine",
            Categories = dto?.Categories ?? new(),
            FeaturedArticles = dto?.FeaturedArticles ?? new(),
            LatestArticles = dto?.LatestArticles ?? new()
        };

        return View(viewModel);
    }
}
