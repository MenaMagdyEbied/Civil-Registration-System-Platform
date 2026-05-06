namespace Civil_Registration_System_Platform.Repositories.Interfaces
{
    public interface IServicesTypeHelperRepository
    {
        //  Reads 
        Task<ServicesTypeHelper?> GetByServiceAndTypeAsync(int serviceTypeEnum, int applicationTypeEnum);
        Task<IEnumerable<ServicesTypeHelper>> GetByServiceTypeAsync(int serviceTypeEnum);
        Task<IEnumerable<ServicesTypeHelper>> GetAllAsync();
        Task AddAsync(ServicesTypeHelper entity);

        Task UpdateAsync(ServicesTypeHelper entity);

        Task DeleteAsync(int serviceTypeEnum, int applicationTypeEnum);

        Task<bool> ExistsAsync(int serviceTypeEnum, int applicationTypeEnum);

        Task SaveChangesAsync();
    }
}
