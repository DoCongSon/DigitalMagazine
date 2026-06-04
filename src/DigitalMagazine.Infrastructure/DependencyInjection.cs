using Microsoft.Extensions.DependencyInjection;
using DigitalMagazine.Application.Home;
using DigitalMagazine.Application.Analytics;
using DigitalMagazine.Infrastructure.Cms.Home;
using DigitalMagazine.Infrastructure.Cms.Analytics;

namespace DigitalMagazine.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IHomePageService, PiranhaHomePageService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        return services;
    }
}
