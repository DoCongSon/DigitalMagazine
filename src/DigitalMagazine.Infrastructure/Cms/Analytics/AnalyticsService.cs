using Microsoft.EntityFrameworkCore;
using Piranha;
using DigitalMagazine.Application.Analytics;
using DigitalMagazine.Infrastructure.Data;
using DigitalMagazine.Infrastructure.Data.Entities;

namespace DigitalMagazine.Infrastructure.Cms.Analytics;

public class AnalyticsService : IAnalyticsService
{
    private readonly AppDbContext _db;
    private readonly IApi _api;

    public AnalyticsService(AppDbContext db, IApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task LogViewAsync(Guid postId, string ipAddress)
    {
        var oneHourAgo = DateTime.UtcNow.AddHours(-1);
        
        // Prevent spam: check if this IP viewed this post in the last 1 hour
        var alreadyViewed = await _db.PageViews
            .AnyAsync(v => v.PostId == postId && v.IpAddress == ipAddress && v.ViewedAt >= oneHourAgo);

        if (!alreadyViewed)
        {
            _db.PageViews.Add(new PageView
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                IpAddress = ipAddress,
                ViewedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<ArticleStatsDto>> GetTopArticlesAsync(int top)
    {
        var today = DateTime.UtcNow.Date;

        // Group by PostId and count views
        var statsQuery = await _db.PageViews
            .GroupBy(v => v.PostId)
            .Select(g => new
            {
                PostId = g.Key,
                TotalViews = g.Count(),
                ViewsToday = g.Count(v => v.ViewedAt >= today)
            })
            .OrderByDescending(s => s.TotalViews)
            .Take(top)
            .ToListAsync();

        var result = new List<ArticleStatsDto>();

        foreach (var stat in statsQuery)
        {
            var post = await _api.Posts.GetByIdAsync(stat.PostId);
            if (post != null)
            {
                result.Add(new ArticleStatsDto
                {
                    PostId = stat.PostId,
                    Title = post.Title,
                    Slug = post.Slug,
                    TotalViews = stat.TotalViews,
                    ViewsToday = stat.ViewsToday
                });
            }
        }

        return result;
    }
}
