using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Helpers;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IApplicationService _applicationService;

        public AppointmentService(
            IAppointmentRepository appointmentRepo,
            IApplicationService applicationService)
        {
            _appointmentRepo = appointmentRepo;
            _applicationService = applicationService;
        }
        // Schedule 
        public async Task<AppointmentSummaryVM> ScheduleAsync(
            int applicationId, DateTime date, TimeSpan time, string adminId)
        {
            
            var existing = await _appointmentRepo.GetByApplicationIdAsync(applicationId);
            if (existing != null)
                await _appointmentRepo.SoftDeleteAsync(existing.AppointmentId);

            var appointment = new Models.Appointment
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

            await _applicationService.ChangeStatusAsync(
                applicationId,
                ApplicationStatus.AppointmentScheduled,
                adminId,
                $"موعدك: {date:yyyy-MM-dd} الساعة {time:hh\\:mm}");

            return MapToSummary(appointment);
        }

        // Status updates

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

        public async Task CancelAsync(int appointmentId, string performedById, string? reason)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null) return;

            appointment.Status = (int)AppointmentStatus.Cancelled;
            appointment.Note = reason;
            await _appointmentRepo.UpdateAsync(appointment);
            await _appointmentRepo.SaveChangesAsync();
        }
        public async Task<IEnumerable<AppointmentSummaryVM>> GetOfficeScheduleAsync(
            int officeId, DateTime date)
        {
            var appointments = await _appointmentRepo.GetByOfficeAndDateAsync(officeId, date);
            return appointments.Select(MapToSummary);
        }

        public async Task<IEnumerable<AppointmentSummaryVM>> GetUserUpcomingAsync(string userId)
        {
            var appointments = await _appointmentRepo.GetUpcomingByUserIdAsync(userId);
            return appointments.Select(MapToSummary);
        }

        // Private helper

        private static AppointmentSummaryVM MapToSummary(Appointment a)
        {
            var appointmentStatus = (AppointmentStatus)a.Status;

            return new AppointmentSummaryVM
            {
                AppointmentId = a.AppointmentId,
                ApplicationNumber = a.Application?.ApplicationNumber ?? string.Empty,
                ServiceTypeName = a.Application != null  ? ((ServiceType)a.Application.ServiceType).ToArabicName(): string.Empty,
                AppointmentDate = a.AppointmentDate,
                OfficeName = a.Application?.Office?.Name ?? string.Empty,
                StatusName = appointmentStatus.ToArabicName()
            };
        }
    }
}
