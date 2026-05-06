using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Helpers;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly IApplicationRepository _appRepo;
        private readonly IOfficeRepository _officeRepo;

        public SuperAdminService(
            IApplicationRepository appRepo,
            IOfficeRepository officeRepo)
        {
            _appRepo = appRepo;
            _officeRepo = officeRepo;
        }

        public async Task<SuperAdminDashboardVM> GetDashboardAsync(
            ApplicationStatus? filterStatus,
            ServiceType? filterService,
            int? filterOfficeId,
            DateTime? dateFrom,
            DateTime? dateTo)
        {
            var allOffices = (await _officeRepo.GetActiveOfficesAsync()).ToList();
            var officeIds  = allOffices.Select(o => o.OfficeId).ToList();
            var statsByOffice = await _appRepo.GetStatsByOfficesAsync(officeIds);
            var todayByOffice = await _appRepo.GetTodayCountByOfficesAsync(officeIds);

            int Count(int officeId, params ApplicationStatus[] sts)
            {
                if (!statsByOffice.TryGetValue(officeId, out var dict)) return 0;
                int total = 0;
                foreach (var s in sts)
                    if (dict.TryGetValue((int)s, out var c)) total += c;
                return total;
            }
            var officeStats = allOffices.Select(o => new OfficeStatsVM
            {
                OfficeId  = o.OfficeId,
                OfficeName   = o.Name,
                GovernorateName   = o.Governorate?.Name ?? string.Empty,
                TotalApplications = Count(o.OfficeId,
                                        ApplicationStatus.Submitted, ApplicationStatus.UnderReview,
                                        ApplicationStatus.AdditionalInfoRequired,
                                        ApplicationStatus.AppointmentScheduled,
                                        ApplicationStatus.MedicalExamPending,
                                        ApplicationStatus.TheoryTestPending,
                                        ApplicationStatus.PracticalTestPending,
                                        ApplicationStatus.Approved,
                                        ApplicationStatus.Rejected,
                                        ApplicationStatus.Issued,
                                        ApplicationStatus.Cancelled),
                PendingCount  = Count(o.OfficeId, ApplicationStatus.Submitted, ApplicationStatus.UnderReview),
                IssuedCount = Count(o.OfficeId, ApplicationStatus.Issued),
                RejectedCount = Count(o.OfficeId, ApplicationStatus.Rejected),
                IsActive  = o.IsActive
            }).ToList();

            var totalToday = todayByOffice.Values.Sum();
            var totalPending  = officeStats.Sum(o => o.PendingCount);
            var totalIssued   = officeStats.Sum(o => o.IssuedCount);
            var totalRejected = officeStats.Sum(o => o.RejectedCount);

            var targetOffices = filterOfficeId.HasValue
                ? allOffices.Where(o => o.OfficeId == filterOfficeId.Value)
                : allOffices;
            var targetOfficeIds = targetOffices.Select(o => o.OfficeId).ToList();

            var apps = await _appRepo.GetFilteredByOfficesAsync(
                targetOfficeIds, filterStatus, filterService);
            if (dateFrom.HasValue) apps = apps.Where(a => a.CreatedAt.Date >= dateFrom.Value.Date);
            if (dateTo.HasValue) apps = apps.Where(a => a.CreatedAt.Date <= dateTo.Value.Date);

            var recentApps = apps.Select(a =>
                {
                    var appStatus      = (ApplicationStatus)a.Status;
                    var appServiceType = (ServiceType)a.ServiceType;
                    var appType        = a.ApplicationType.HasValue
                                            ? (ApplicationType?)a.ApplicationType.Value
                                            : null;
                    return new ApplicationSummaryAdminVM
                    {
                        ApplicationId = a.ApplicationId,
                        ApplicationNumber = a.ApplicationNumber,
                        ApplicantName = a.UserAccount?.FullName ?? string.Empty,
                        ApplicantNationalId = a.UserAccount?.NationalID ?? string.Empty,
                        ServiceTypeName = appServiceType.ToArabicName(),
                        ApplicationTypeName = appType.HasValue ? appType.Value.ToArabicName() : string.Empty,
                        StatusName = appStatus.ToArabicName(),
                        StatusColor = appStatus.ToStatusColor(),
                        CreatedAt = a.CreatedAt
                    };
                }).ToList();

            var statsByService = Enum.GetValues<ServiceType>().Select(st =>
            {
                var forService = recentApps.Where(a => a.ServiceTypeName == st.ToArabicName()).ToList();
                return new ServiceStatsVM
                {
                    ServiceTypeName = st.ToArabicName(),
                    TotalApplications = forService.Count,
                    PendingCount = forService.Count(a => a.StatusName == ApplicationStatus.Submitted.ToArabicName()
                                                           || a.StatusName == ApplicationStatus.UnderReview.ToArabicName()),
                    ApprovedCount = forService.Count(a => a.StatusName == ApplicationStatus.Approved.ToArabicName()),
                    RejectedCount = forService.Count(a => a.StatusName == ApplicationStatus.Rejected.ToArabicName()),
                    IssuedCount  = forService.Count(a => a.StatusName == ApplicationStatus.Issued.ToArabicName())
                };
            }).Where(s => s.TotalApplications > 0).ToList();

            return new SuperAdminDashboardVM
            {
                TotalApplicationsToday = totalToday,
                TotalPendingReview = totalPending,
                TotalIssuedToday = totalIssued,
                TotalRejectedToday = totalRejected,
                TotalActiveOffices = allOffices.Count,
                RecentApplications = recentApps.OrderByDescending(a => a.CreatedAt).Take(50).ToList(),
                StatsByService = statsByService,
                StatsByOffice = officeStats,
                FilterStatus = filterStatus,
                FilterService = filterService,
                FilterOfficeId = filterOfficeId,
                FilterDateFrom = dateFrom,
                FilterDateTo  = dateTo
            };
        }
    }
}
