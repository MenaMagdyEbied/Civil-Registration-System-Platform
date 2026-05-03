using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<Appointment?> GetByApplicationIdAsync(int applicationId);
        Task<IEnumerable<Appointment>> GetByOfficeAndDateAsync(int officeId, DateTime date);
        Task<IEnumerable<Appointment>> GetUpcomingByUserIdAsync(string userId);
    }
}
