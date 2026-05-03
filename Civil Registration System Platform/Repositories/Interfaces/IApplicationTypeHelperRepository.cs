namespace Civil_Registration_System_Platform.Repositories.Interfaces
{
    public interface IApplicationTypeHelperRepository
    {
        Task<ApplicationTypeHelper?> GetByApplicationTypeAsync(int appTypeEnum);
        Task<IEnumerable<ApplicationTypeHelper>> GetAllAsync();
    }
}
