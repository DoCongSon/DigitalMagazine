using DigitalMagazine.Application.Home;

namespace DigitalMagazine.Web.Models.Home;

public class HomeViewModel
{
    public string PageTitle { get; set; } = string.Empty;
    public List<HomeCategoryDto> Categories { get; set; } = new();
    public List<HomeArticleDto> FeaturedArticles { get; set; } = new();
    public List<HomeArticleDto> LatestArticles { get; set; } = new();
}
