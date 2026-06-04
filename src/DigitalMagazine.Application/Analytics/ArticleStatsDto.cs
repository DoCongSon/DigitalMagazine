namespace DigitalMagazine.Application.Analytics;

public class ArticleStatsDto
{
    public Guid PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int TotalViews { get; set; }
    public int ViewsToday { get; set; }
}
