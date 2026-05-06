namespace Civil_Registration_System_Platform.Services.Interfaces
{
    public interface IHomePageService
    {
        Task<HomePageVM> GetHomePageDataAsync();
    }
}
