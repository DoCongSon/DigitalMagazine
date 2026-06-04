using Microsoft.EntityFrameworkCore;
using Piranha;
using Piranha.AspNetCore.Identity.SQLServer;
using Piranha.AttributeBuilder;
using Piranha.Data.EF.SQLServer;
using DigitalMagazine.Infrastructure;
using DigitalMagazine.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("piranha");

// === Piranha CMS Services ===
builder.AddPiranha(options =>
{
    options.UseCms();
    options.UseManager();
    options.UseFileStorage(naming: Piranha.Local.FileStorageNaming.UniqueFolderNames);
    options.UseImageSharp();
    options.UseTinyMCE();
    options.UseMemoryCache();
    
    // Dùng SQL Server
    options.UseEF<SQLServerDb>(db => db.UseSqlServer(connectionString));
    options.UseIdentityWithSeed<IdentitySQLServerDb>(db => db.UseSqlServer(connectionString));
});

// === App Custom DB ===
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// === Infrastructure DI ===
builder.Services.AddInfrastructure();

// === MVC ===
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

// === Piranha Middleware ===
app.UsePiranha(options =>
{
    App.Init(options.Api);
    
    // Inject Custom CSS for Manager (Offline Gravatar fix)
    App.Modules.Get<Piranha.Manager.Module>().Styles.Add("~/assets/css/manager-custom.css");
    
    // Đăng ký toàn bộ Models từ DigitalMagazine.CMS
    new ContentTypeBuilder(options.Api)
        .AddAssembly(typeof(DigitalMagazine.CMS.Models.StandardPage).Assembly)
        .Build()
        .DeleteOrphans();

    var analyticsGroup = new Piranha.Manager.MenuItem
    {
        InternalId = "AnalyticsGroup",
        Name = "Báo Cáo",
        Css = "fas fa-chart-bar"
    };
    
    analyticsGroup.Items.Add(new Piranha.Manager.MenuItem
    {
        InternalId = "Analytics",
        Name = "Thống Kê",
        Route = "~/manager/analytics",
        Css = "fas fa-chart-line"
    });

    Piranha.Manager.Menu.Items.Insert(2, analyticsGroup);

    options.UseManager();
    options.UseTinyMCE();
    options.UseIdentity();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
