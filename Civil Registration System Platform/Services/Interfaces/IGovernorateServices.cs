namespace Civil_Registration_System_Platform.Services.Interfaces
{
    public interface IGovernorateServices
    {
        Task<List<GovernorateGetAll>> GetAllGovernoratesAsync();

    }
}
