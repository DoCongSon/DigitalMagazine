namespace DigitalMagazine.Application.Home;

public interface IHomePageService
{
    Task<HomePageDto?> GetHomePageAsync();
}
