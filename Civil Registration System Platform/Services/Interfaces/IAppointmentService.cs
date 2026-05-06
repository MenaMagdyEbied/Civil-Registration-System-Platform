using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Interfaces
{
    public interface IAppointmentService
    {     
        Task<AppointmentSummaryVM> ScheduleAsync(
            int applicationId,
            DateTime date,
            TimeSpan time,
            string adminId,
            ApplicationStatus? targetStatus = null);
        Task MarkAsCompletedAsync(int appointmentId, string adminId);

        Task MarkAsNoShowAsync(int appointmentId, string adminId);
        Task CancelAsync(int appointmentId, string performedById, string? reason);

        // Admin
        Task<IEnumerable<AppointmentSummaryVM>> GetOfficeScheduleAsync(int officeId, DateTime date);

        // Client dashboard
        Task<IEnumerable<AppointmentSummaryVM>> GetUserUpcomingAsync(string userId);
    }
}
