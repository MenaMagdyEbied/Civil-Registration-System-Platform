using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Helpers;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    // عمليات الأدمن على طلبات مكتبه
 
    public class AdminApplicationService : IAdminApplicationService
    {
        private readonly IApplicationRepository _appRepo;
        private readonly ITimelineEntryRepository _timelineRepo;
        private readonly IPricingService _pricingService;

        public AdminApplicationService(
            IApplicationRepository appRepo,
            ITimelineEntryRepository timelineRepo,
            IPricingService pricingService)
        {
            _appRepo = appRepo;
            _timelineRepo   = timelineRepo;
            _pricingService = pricingService;
        }

        // ─── داشبورد المكتب 

        public async Task<AdminDashboardVM> GetOfficeDashboardAsync(
            int officeId, ApplicationStatus? filterStatus, ServiceType? filterService)
            => await GetOfficesDashboardAsync(new[] { officeId }, filterStatus, filterService);

        public async Task<AdminDashboardVM> GetOfficesDashboardAsync(
            IEnumerable<int> officeIds, ApplicationStatus? filterStatus, ServiceType? filterService)
        {
            var ids = officeIds.Distinct().ToList();
            var statusStats = await _appRepo.GetStatsByOfficesAsync(ids);
            var todayStats = await _appRepo.GetTodayCountByOfficesAsync(ids);

            int Count(params ApplicationStatus[] statuses)
            {
                var statusIds = statuses.Select(s => (int)s).ToArray();
                return statusStats.Values.Sum(dict =>
                    dict.Where(row => statusIds.Contains(row.Key)).Sum(row => row.Value));
            }

            var apps = await GetOfficesApplicationsAsync(ids, filterStatus, filterService);

            return new AdminDashboardVM
            {
                TodayApplications = todayStats.Values.Sum(),
                PendingReview = Count(ApplicationStatus.Submitted, ApplicationStatus.UnderReview),
                IssuedToday = Count(ApplicationStatus.Issued),
                RejectedToday = Count(ApplicationStatus.Rejected),
                Applications = apps.ToList(),
                FilterStatus  = filterStatus,
                FilterService = filterService
            };
        }
        public async Task<IEnumerable<ApplicationSummaryAdminVM>> GetOfficeApplicationsAsync(
            int officeId, ApplicationStatus? status, ServiceType? serviceType)
            => await GetOfficesApplicationsAsync(new[] { officeId }, status, serviceType);

        public async Task<IEnumerable<ApplicationSummaryAdminVM>> GetOfficesApplicationsAsync(
            IEnumerable<int> officeIds, ApplicationStatus? status, ServiceType? serviceType)
        {
            var apps = await _appRepo.GetFilteredByOfficesAsync(officeIds, status, serviceType);

            return apps.Select(a =>
            {
                var appStatus  = (ApplicationStatus)a.Status;
                var appServiceType = (ServiceType)a.ServiceType;
                var appType = a.ApplicationType.HasValue
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
            });
        }

        //  تفاصيل طلب  

        public async Task<ReviewApplicationVM?> GetReviewDetailsAsync(int applicationId)
        {
            var app = await _appRepo.GetWithFullDetailsAsync(applicationId);
            if (app == null) return null;

            var appStatus = (ApplicationStatus)app.Status;
            var appServiceType = (ServiceType)app.ServiceType;
            var appType = app.ApplicationType.HasValue
                                 ? (ApplicationType?)app.ApplicationType.Value
                                 : null;

            var latestAppointment = app.Appointments?
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.AppointmentDate)
                .FirstOrDefault();

            return new ReviewApplicationVM
            {
                ApplicationId = app.ApplicationId,
                ApplicationNumber = app.ApplicationNumber,
                ServiceTypeName = appServiceType.ToArabicName(),
                ApplicationTypeName = appType.HasValue ? appType.Value.ToArabicName() : string.Empty,
                StatusName = appStatus.ToArabicName(),
                CreatedAt = app.CreatedAt,
                Price = await _pricingService.CalculatePriceAsync(appServiceType, appType),
                ApplicantName = app.UserAccount?.FullName ?? string.Empty,
                ApplicantNationalId = app.UserAccount?.NationalID ?? string.Empty,
                ApplicantPhone = app.UserAccount?.EGPhoneNumber ?? string.Empty,
                ApplicantGovernorateName = app.UserAccount?.Governorate?.Name ?? string.Empty,
                OfficeName = app.Office?.Name ?? string.Empty,
                OfficeId = app.OfficeId,
                AdminNotes = app.Note,
                RejectionReason = appStatus == ApplicationStatus.Rejected ? app.Note : null,
                AppointmentDate = latestAppointment?.AppointmentDate.Date,
                AppointmentTime = latestAppointment?.AppointmentDate.TimeOfDay,

                Documents = app.ApplicationDocuments?
                    .Where(d => !d.IsDeleted)
                    .Select(d => new DocumentVM
                    {
                        DocumentId = d.ApplicationDocumentsId,
                        Name = d.Name,
                        DocumentPath = d.DocumentPath,
                        UploadedAt = d.UploadedAt
                    }).ToList() ?? new(),

                Timeline = app.TimelineEntries?
                    .Where(t => !t.IsDeleted)
                    .OrderBy(t => t.Timestamp)
                    .Select(t => new TimelineItemVM
                    {
                        StatusName = ((ApplicationStatus)t.Status).ToArabicName(),
                        Description  = t.Description ?? string.Empty,
                        Timestamp  = t.Timestamp,
                        PerformedByName = t.PerformedBy?.FullName ?? string.Empty
                    }).ToList() ?? new()
            };
        }
        public async Task ChangeStatusAsync(
            int applicationId, ApplicationStatus newStatus,
            string adminId, string? note = null)
        {
            var app = await _appRepo.GetByIdAsync(applicationId);
            if (app == null) return;

            app.Status = (int)newStatus;
            app.UpdatedAt = DateTime.UtcNow;
            app.ReviewedById = adminId;
            app.ReviewedAt = DateTime.UtcNow;

            if (note != null)
                app.Note = note;

            await _appRepo.UpdateAsync(app);

            var description = newStatus switch
            {
                ApplicationStatus.UnderReview    => "جاري مراجعة طلبك",
                ApplicationStatus.AdditionalInfoRequired => $"مطلوب مستندات إضافية: {note}",
                ApplicationStatus.AppointmentScheduled  => "تم تحديد موعدك، تحقق من التفاصيل",
                ApplicationStatus.MedicalExamPending    => "سيتم تحديد موعد الكشف الطبي قريباً",
                ApplicationStatus.TheoryTestPending     => "سيتم تحديد موعد الاختبار النظري قريباً",
                ApplicationStatus.PracticalTestPending  => "سيتم تحديد موعد الاختبار العملي قريباً",
                ApplicationStatus.Approved  => "تمت الموافقة على طلبك",
                ApplicationStatus.Rejected  => $"تم رفض طلبك. السبب: {note}",
                ApplicationStatus.Issued     => "تم إصدار الوثيقة، يمكنك استلامها",
                ApplicationStatus.Cancelled   => "تم إلغاء الطلب",
                _                                       => "تم تحديث حالة طلبك"
            };

            await _timelineRepo.AddEntryAsync(applicationId, newStatus, description, adminId);
            await _appRepo.SaveChangesAsync();
        }

        public async Task ApproveApplicationAsync(int applicationId, string adminId, string? notes)
            => await ChangeStatusAsync(applicationId, ApplicationStatus.Approved, adminId, notes);

        public async Task RejectApplicationAsync(int applicationId, string adminId, string reason)
            => await ChangeStatusAsync(applicationId, ApplicationStatus.Rejected, adminId, reason);

        public async Task RequestAdditionalInfoAsync(int applicationId, string adminId, string details)
            => await ChangeStatusAsync(applicationId, ApplicationStatus.AdditionalInfoRequired, adminId, details);

        public async Task IssueApplicationAsync(int applicationId, string adminId)
            => await ChangeStatusAsync(applicationId, ApplicationStatus.Issued, adminId);
    }
}
