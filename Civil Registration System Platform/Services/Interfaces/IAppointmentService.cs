namespace Civil_Registration_System_Platform.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<Appointment> ScheduleAsync( int applicationId, DateTime date, TimeSpan time, string adminId);

        Task MarkAsCompletedAsync(int appointmentId, string adminId);
        Task MarkAsNoShowAsync(int appointmentId, string adminId);
        Task CancelAsync(int appointmentId, string performedById, string? reason);
        Task<IEnumerable<Appointment>> GetOfficeScheduleAsync(int officeId, DateTime date);
        Task<IEnumerable<Appointment>> GetUserUpcomingAsync(string userId);
    }
}
