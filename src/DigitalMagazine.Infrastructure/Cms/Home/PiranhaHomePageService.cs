using Piranha;
using Piranha.Models;
using DigitalMagazine.Application.Home;
using DigitalMagazine.CMS.Models;

namespace DigitalMagazine.Infrastructure.Cms.Home;

public class PiranhaHomePageService : IHomePageService
{
    private readonly IApi _api;
    
    public PiranhaHomePageService(IApi api) => _api = api;

    public async Task<HomePageDto?> GetHomePageAsync()
    {
        var pages = await _api.Pages.GetAllAsync();
        var archivePage = pages.FirstOrDefault(p => p.TypeId == "StandardArchive");
        if (archivePage == null) return null;

        var categories = await _api.Posts.GetAllCategoriesAsync(archivePage.Id);
        var posts = await _api.Posts.GetAllAsync<StandardPost>(archivePage.Id);
        
        var articleDtos = posts
            .Where(p => p.Published.HasValue)
            .OrderByDescending(p => p.Published)
            .Select(p => new HomeArticleDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                Excerpt = p.Excerpt ?? string.Empty,
                ImageUrl = p.PrimaryImage?.Media?.PublicUrl ?? string.Empty,
                CategoryTitle = p.Category?.Title ?? string.Empty,
                CategorySlug = p.Category?.Slug ?? string.Empty,
                Published = p.Published
            })
            .ToList();

        return new HomePageDto
        {
            PageTitle = archivePage.Title,
            Categories = categories.Select(c => new HomeCategoryDto
            {
                Id = c.Id,
                Title = c.Title,
                Slug = c.Slug
            }).ToList(),
            FeaturedArticles = articleDtos.Take(5).ToList(),
            LatestArticles = articleDtos.Skip(5).Take(10).ToList()
        };
    }
}
