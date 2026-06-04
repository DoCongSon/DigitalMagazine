using Microsoft.EntityFrameworkCore;
using DigitalMagazine.Infrastructure.Data.Entities;

namespace DigitalMagazine.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<PageView> PageViews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<PageView>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.PostId);
            entity.HasIndex(e => e.ViewedAt);
        });
    }
}
