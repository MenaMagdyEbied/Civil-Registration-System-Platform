namespace Civil_Registration_System_Platform.Repositories.Interfaces
{
    public interface IServicesTypeHelperRepository
    {
        Task<ServicesTypeHelper?> GetByServiceTypeAsync(int serviceTypeEnum);
        Task<IEnumerable<ServicesTypeHelper>> GetAllAsync();
    }
}
