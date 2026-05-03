using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IApplicationService _applicationService;

        public AppointmentService(  IAppointmentRepository appointmentRepo,IApplicationService applicationService)
        {
            _appointmentRepo = appointmentRepo;
            _applicationService = applicationService;
        }

        public async Task<Appointment> ScheduleAsync( int applicationId, DateTime date, TimeSpan time, string adminId)
        {
            var app = await _appointmentRepo.GetByApplicationIdAsync(applicationId);

            if (app != null)
                await _appointmentRepo.SoftDeleteAsync(app.AppointmentId);

            var appointment = new Appointment
            {
                ApplicationId = applicationId,
                AppointmentDate = date.Date + time,
                Status = (int)AppointmentStatus.Scheduled,
                ScheduledById = adminId,
                UserAccountId = adminId,
                IsDeleted = false
            };

            await _appointmentRepo.AddAsync(appointment);
            await _appointmentRepo.SaveChangesAsync();

            await _applicationService.ChangeStatusAsync( applicationId, ApplicationStatus.AppointmentScheduled, adminId,
                $"موعدك: {date:yyyy-MM-dd} الساعة {time:hh\\:mm}");

            return appointment;
        }

        public async Task MarkAsCompletedAsync(int appointmentId, string adminId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null) return;

            appointment.Status = (int)AppointmentStatus.Completed;
            await _appointmentRepo.UpdateAsync(appointment);
            await _appointmentRepo.SaveChangesAsync();
        }

        public async Task MarkAsNoShowAsync(int appointmentId, string adminId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null) return;

            appointment.Status = (int)AppointmentStatus.NoShow;
            await _appointmentRepo.UpdateAsync(appointment);
            await _appointmentRepo.SaveChangesAsync();
        }

        public async Task CancelAsync(
            int appointmentId, string performedById, string? reason)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null) return;

            appointment.Status = (int)AppointmentStatus.Cancelled;
            appointment.Note = reason;
            await _appointmentRepo.UpdateAsync(appointment);
            await _appointmentRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> GetOfficeScheduleAsync(
            int officeId, DateTime date)
            => await _appointmentRepo.GetByOfficeAndDateAsync(officeId, date);

        public async Task<IEnumerable<Appointment>> GetUserUpcomingAsync(string userId)
            => await _appointmentRepo.GetUpcomingByUserIdAsync(userId);
    }
}
