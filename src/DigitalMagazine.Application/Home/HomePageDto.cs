namespace DigitalMagazine.Application.Home;

public class HomePageDto
{
    public string PageTitle { get; set; } = string.Empty;
    public List<HomeCategoryDto> Categories { get; set; } = new();
    public List<HomeArticleDto> FeaturedArticles { get; set; } = new();
    public List<HomeArticleDto> LatestArticles { get; set; } = new();
}

public class HomeCategoryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}

public class HomeArticleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string CategoryTitle { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;
    public DateTime? Published { get; set; }
}
