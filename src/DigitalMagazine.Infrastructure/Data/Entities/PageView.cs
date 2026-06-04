namespace DigitalMagazine.Infrastructure.Data.Entities;

public class PageView
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public DateTime ViewedAt { get; set; }
}
