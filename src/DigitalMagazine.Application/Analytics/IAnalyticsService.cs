namespace DigitalMagazine.Application.Analytics;

public interface IAnalyticsService
{
    Task LogViewAsync(Guid postId, string ipAddress);
    Task<List<ArticleStatsDto>> GetTopArticlesAsync(int top);
}
