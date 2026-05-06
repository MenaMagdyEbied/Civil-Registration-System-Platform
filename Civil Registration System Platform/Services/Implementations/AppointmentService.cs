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
        private readonly IApplicationRepository _appRepo;
        private readonly IAdminApplicationService _adminApplicationService;

        public AppointmentService(
            IAppointmentRepository appointmentRepo,
            IApplicationRepository appRepo,
            IAdminApplicationService adminApplicationService)
        {
            _appointmentRepo = appointmentRepo;
            _appRepo = appRepo;
            _adminApplicationService = adminApplicationService;
        }


        public async Task<AppointmentSummaryVM> ScheduleAsync(
            int applicationId,
            DateTime date,
            TimeSpan time,
            string adminId,
            ApplicationStatus? targetStatus = null)
        {
            var existing = await _appointmentRepo.GetByApplicationIdAsync(applicationId);
            if (existing != null)
                await _appointmentRepo.SoftDeleteAsync(existing.AppointmentId);

            var appointment = new Models.Appointment
            {
                ApplicationId   = applicationId,
                AppointmentDate = date.Date + time,
                Status  = (int)AppointmentStatus.Scheduled,
                ScheduledById = adminId,
                UserAccountId  = adminId,
                IsDeleted  = false
            };

            await _appointmentRepo.AddAsync(appointment);
            await _appointmentRepo.SaveChangesAsync();
            var newStatus = targetStatus ?? ApplicationStatus.AppointmentScheduled;

            var statusLabel = newStatus.ToArabicName();
            await _adminApplicationService.ChangeStatusAsync(
                applicationId,
                newStatus,
                adminId,
                $"تم جدولة {statusLabel} في {date:yyyy-MM-dd} الساعة {time:hh\\:mm}");

            return MapToSummary(appointment);
        }


        public async Task MarkAsCompletedAsync(int appointmentId, string adminId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null) return;

            appointment.Status = (int)AppointmentStatus.Completed;
            await _appointmentRepo.UpdateAsync(appointment);
            await _appointmentRepo.SaveChangesAsync();

            var app = await _appRepo.GetByIdAsync(appointment.ApplicationId);
            if (app == null) return;

            var currentStatus = (ApplicationStatus)app.Status;
            var nextStatus = currentStatus switch
            {
                ApplicationStatus.MedicalExamPending => ApplicationStatus.TheoryTestPending,
                ApplicationStatus.TheoryTestPending      => ApplicationStatus.PracticalTestPending,
                ApplicationStatus.PracticalTestPending   => ApplicationStatus.Approved,
                ApplicationStatus.AppointmentScheduled   => ApplicationStatus.Approved,
                _                                        => (ApplicationStatus?)null
            };

            if (nextStatus.HasValue)
            {
                var note = currentStatus switch
                {
                    ApplicationStatus.MedicalExamPending   => "تم اجتياز الكشف الطبي — جاري الانتقال للاختبار النظري",
                    ApplicationStatus.TheoryTestPending    => "تم اجتياز الاختبار النظري — جاري الانتقال للاختبار العملي",
                    ApplicationStatus.PracticalTestPending => "تم اجتياز الاختبار العملي — جاري المراجعة النهائية",
                    _                                      => "تم تأكيد حضور الموعد"
                };

                await _adminApplicationService.ChangeStatusAsync(
                    appointment.ApplicationId,
                    nextStatus.Value,
                    adminId,
                    note);
            }
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
            appointment.Note   = reason;
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

        private static AppointmentSummaryVM MapToSummary(Models.Appointment a)
        {
            var appointmentStatus = (AppointmentStatus)a.Status;

            return new AppointmentSummaryVM
            {
                AppointmentId = a.AppointmentId,
                ApplicationNumber = a.Application?.ApplicationNumber ?? string.Empty,
                ServiceTypeName   = a.Application != null
                                      ? ((ServiceType)a.Application.ServiceType).ToArabicName()
                                      : string.Empty,
                AppointmentDate = a.AppointmentDate,
                OfficeName   = a.Application?.Office?.Name ?? string.Empty,
                StatusName    = appointmentStatus.ToArabicName()
            };
        }
    }
}
