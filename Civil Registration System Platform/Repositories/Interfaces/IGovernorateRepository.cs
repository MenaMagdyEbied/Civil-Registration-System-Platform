namespace Civil_Registration_System_Platform.Repositories.Interfaces
{
    public interface IGovernorateRepository : IGenericRepository<Governorate>
    {
        Task<IEnumerable<Governorate>> GetActiveGovernoratesAsync();
      
    }
}
