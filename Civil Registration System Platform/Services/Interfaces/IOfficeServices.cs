namespace Civil_Registration_System_Platform.Services.Interfaces
{
    public interface IOfficeServices
    {
        Task<IEnumerable<OfficeGovernrate>> GetByGovernorateIdAsync(int governorateId);
    }
}
