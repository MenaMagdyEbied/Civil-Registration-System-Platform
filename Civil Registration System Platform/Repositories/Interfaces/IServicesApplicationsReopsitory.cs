namespace Civil_Registration_System_Platform.Repositories.Interfaces
{
    public interface IServicesApplicationsReopsitory
    {
        Task<List<ServicesTypeHelper>> GetAll();
        Task<ServicesTypeHelper> GetByIds(int servicesType, int ApplicationType);
        Task<ServicesTypeHelper> Update(ServicesTypeHelper servicesTypeHelper);

    }
}
