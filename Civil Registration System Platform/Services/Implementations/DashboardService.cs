using Civil_Registration_System_Platform.Account.AccountRepository;
using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Helpers;
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    
    // داشبورد المواطن
    public class DashboardService : IDashboardService
    {
        private readonly IApplicationService _applicationService;
        private readonly IAppointmentService _appointmentService;
        private readonly IUserAccountRepository _userRepo;

        public DashboardService(
            IApplicationService applicationService,
            IAppointmentService appointmentService,
            IUserAccountRepository userRepo)
        {
            _applicationService = applicationService;
            _appointmentService = appointmentService;
            _userRepo = userRepo;
        }

        public async Task<DashboardVM> GetCitizenDashboardAsync(string userId)
        {
            var user = await _userRepo.GetMyAccount();

            var apps = (await _applicationService.GetUserApplicationsAsync(userId)).ToList();

            var appointments = (await _appointmentService.GetUserUpcomingAsync(userId)).ToList();

            var issuedName = ApplicationStatus.Issued.ToArabicName();
            var approvedName  = ApplicationStatus.Approved.ToArabicName();
            var rejectedName  = ApplicationStatus.Rejected.ToArabicName();
            var cancelledName = ApplicationStatus.Cancelled.ToArabicName();

            var totalApplications = apps.Count;
            var pendingApplications = apps.Count(a =>
                                            a.StatusName != issuedName
                                         && a.StatusName != rejectedName
                                         && a.StatusName != cancelledName);
            var approvedApplications  = apps.Count(a =>
                                            a.StatusName == issuedName
                                         || a.StatusName == approvedName);
            var rejectedApplications = apps.Count(a => a.StatusName == rejectedName);

            return new DashboardVM
            {
                UserName  = user?.FullName ?? string.Empty,
                TotalApplications    = totalApplications,
                PendingApplications  = pendingApplications,
                ApprovedApplications = approvedApplications,
                RejectedApplications = rejectedApplications,
                RecentApplications   = apps.Take(10).ToList(),
                UpcomingAppointments = appointments.ToList()
            };
        }
    }
}
