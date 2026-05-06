namespace Civil_Registration_System_Platform.Services.Interfaces
{
    public interface IServicesApplicationsServices
    {
        Task<List<ServicesApplicationsVM>> GetAllServicesWithApplications();
        Task<string> UpdateServiceApplication(ServicesApplicationsAddOrUpdateVM servicesApplicationsAddOrUpdateVM);
    }
}
