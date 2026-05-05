namespace Civil_Registration_System_Platform.Repositories.Interfaces
{
    public interface IOfficeRepository : IGenericRepository<Office>
    {
        Task<IEnumerable<Office>> GetByGovernorateIdAsync(int governorateId);

        Task<Office?> GetWithGovernorateAsync(int officeId);
  
        Task<IEnumerable<Office>> GetActiveOfficesAsync();
    }
}
