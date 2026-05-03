using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Interfaces
{
    public interface IApplicationDocumentRepository : IGenericRepository<ApplicationDocuments>
    {
        Task<IEnumerable<ApplicationDocuments>> GetByApplicationIdAsync(int applicationId);

        Task SoftDeleteByApplicationIdAsync(int applicationId);
    }
}
