using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Helpers;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel;
using Civil_Registration_System_Platform.ViewModel.Application;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _appRepo;
        private readonly IApplicationDocumentRepository _docRepo;
        private readonly ITimelineEntryRepository _timelineRepo;
        private readonly IAppointmentRepository _appointmentRepo;
      //  private readonly IPricingService _pricingService;
        private readonly IFileService _fileService;

        public ApplicationService(
            IApplicationRepository appRepo,
            IApplicationDocumentRepository docRepo,
            ITimelineEntryRepository timelineRepo,
            IAppointmentRepository appointmentRepo,
     //       IPricingService pricingService,
            IFileService fileService)
        {
            _appRepo = appRepo;
            _docRepo = docRepo;
            _timelineRepo = timelineRepo;
            _appointmentRepo = appointmentRepo;
      //      _pricingService = pricingService;
            _fileService = fileService;
        }

        public async Task<string> SubmitApplicationAsync(ApplyViewModel model, string userId)
        {
            var appNumber = await _appRepo.GenerateApplicationNumberAsync();
          //  await _pricingService.CalculatePriceAsync(model.ServiceType, model.ApplicationType);

            var application = new Models.Application
            {
                ApplicationNumber = appNumber,
                ServiceType = (int)model.ServiceType,
                ApplicationType = model.ApplicationType.HasValue ? (int)model.ApplicationType.Value : null,
                Status = (int)ApplicationStatus.Submitted,
                OfficeId = model.OfficeId,
                UserAccountId = userId,
                Note = model.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _appRepo.AddAsync(application);
            await _appRepo.SaveChangesAsync();

            if (model.Documents != null && model.Documents.Any())
            {
                foreach (var file in model.Documents)
                {
                    if (file == null || file.Length == 0) continue;

                    var filePath = await _fileService.UploadDocumentAsync(file, application.ApplicationId);

                    var doc = new Models.ApplicationDocuments
                    {
                        ApplicationId = application.ApplicationId,
                        Name = file.FileName,
                        DocumentPath = filePath,
                        UploadedAt = DateTime.UtcNow,
                        UserAccountId = userId,
                        IsDeleted = false
                    };

                    await _docRepo.AddAsync(doc);
                }
            }

            await _timelineRepo.AddEntryAsync(application.ApplicationId,ApplicationStatus.Submitted,
                "تم استلام طلبك بنجاح، سيتم مراجعته قريباً",userId);
            await _appRepo.SaveChangesAsync();

            return appNumber;
        }

        public async Task<ApplicationTrackResultVM?> TrackApplicationAsync(string applicationNumber)
        {
            var app = await _appRepo.GetByApplicationNumberAsync(applicationNumber);
            if (app == null) return null;

            return await MapToTrackResultAsync(app);
        }

        public async Task<IEnumerable<ApplicationSummaryClientVM>> GetUserApplicationsAsync(string userId)
        {
            var apps = await _appRepo.GetByUserIdAsync(userId);

            return apps.Select(a =>
            {
                var status = (ApplicationStatus)a.Status;
                var serviceType = (ServiceType)a.ServiceType;

                return new ApplicationSummaryClientVM
                {
                    ApplicationId = a.ApplicationId,
                    ApplicationNumber = a.ApplicationNumber,
                    ServiceTypeName = serviceType.ToArabicName(),
                    StatusName = status.ToArabicName(),
                    StatusColor = status.ToStatusColor(),
                    CreatedAt = a.CreatedAt,
                    OfficeName = a.Office?.Name ?? string.Empty
                };
            });
        }

        public async Task<ApplicationTrackResultVM?> GetApplicationDetailsAsync(int applicationId, string userId)
        {
            var app = await _appRepo.GetWithFullDetailsAsync(applicationId);

            if (app == null || app.UserAccountId != userId)
                return null;

            return await MapToTrackResultAsync(app);
        }

        public async Task ChangeStatusAsync(
            int applicationId, ApplicationStatus newStatus,string adminId, string? note = null)
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
                ApplicationStatus.UnderReview => "جاري مراجعة طلبك",
                ApplicationStatus.AdditionalInfoRequired => $"مطلوب مستندات إضافية: {note}",
                ApplicationStatus.AppointmentScheduled => "تم تحديد موعدك، تحقق من التفاصيل",
                ApplicationStatus.MedicalExamPending => "سيتم تحديد موعد الكشف الطبي قريباً",
                ApplicationStatus.TheoryTestPending => "سيتم تحديد موعد الاختبار النظري قريباً",
                ApplicationStatus.PracticalTestPending => "سيتم تحديد موعد الاختبار العملي قريباً",
                ApplicationStatus.Approved => "تمت الموافقة على طلبك",
                ApplicationStatus.Rejected => $"تم رفض طلبك. السبب: {note}",
                ApplicationStatus.Issued => "تم إصدار الوثيقة، يمكنك استلامها",
                ApplicationStatus.Cancelled => "تم إلغاء الطلب",
                _ => "تم تحديث حالة طلبك"
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

        public async Task CancelApplicationAsync(int applicationId, string userId)
        {
            var app = await _appRepo.GetByIdAsync(applicationId);

            if (app == null || app.UserAccountId != userId) return;

            if (app.Status == (int)ApplicationStatus.Approved ||
                app.Status == (int)ApplicationStatus.Issued) return;

            await ChangeStatusAsync(applicationId, ApplicationStatus.Cancelled, userId);
        }

        public async Task<IEnumerable<ApplicationSummaryAdminVM>> GetOfficeApplicationsAsync(
            int officeId, ApplicationStatus? status, ServiceType? serviceType)
        {
            var apps = await _appRepo.GetFilteredAsync(officeId, status, serviceType);

            return apps.Select(a =>
            {
                var appStatus = (ApplicationStatus)a.Status;
                var appServiceType = (ServiceType)a.ServiceType;
                var appType = a.ApplicationType.HasValue? (ApplicationType?)a.ApplicationType.Value: null;

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

        public async Task<AdminDashboardVM> GetOfficeDashboardAsync(
            int officeId, ApplicationStatus? filterStatus, ServiceType? filterService)
        {
            var (today, pendingReview, issued, rejected) = await GetOfficeDashboardStatsAsync(officeId);
            var apps = await GetOfficeApplicationsAsync(officeId, filterStatus, filterService);

            return new AdminDashboardVM
            {
                TodayApplications = today,
                PendingReview = pendingReview,
                IssuedToday = issued,
                RejectedToday = rejected,
                Applications = apps.Select(a => new ApplicationSummaryAdminVM
                {
                    ApplicationId = a.ApplicationId,
                    ApplicationNumber = a.ApplicationNumber,
                    ServiceTypeName = a.ServiceTypeName,
                    StatusName = a.StatusName,
                    StatusColor = a.StatusColor,
                    CreatedAt = a.CreatedAt
                }).ToList(),
                FilterStatus = filterStatus,
                FilterService = filterService
            };
        }
        public async Task<ReviewApplicationVM?> GetReviewDetailsAsync(int applicationId)
        {
            var app = await _appRepo.GetWithFullDetailsAsync(applicationId);
            if (app == null) return null;

            var appStatus = (ApplicationStatus)app.Status;
            var appServiceType = (ServiceType)app.ServiceType;
            var appType = app.ApplicationType.HasValue ? (ApplicationType?)app.ApplicationType.Value: null;

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
            //    Price = await _pricingService.CalculatePriceAsync(appServiceType, appType),
                ApplicantName = app.UserAccount?.FullName ?? string.Empty,
                ApplicantNationalId = app.UserAccount?.NationalID ?? string.Empty,
                ApplicantPhone = app.UserAccount?.EGPhoneNumber ?? string.Empty,
                ApplicantGovernorateName = app.UserAccount?.Governorate?.Name ?? string.Empty,
                OfficeName = app.Office?.Name ?? string.Empty,
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
                        Description = t.Description ?? string.Empty,
                        Timestamp = t.Timestamp,
                        PerformedByName = t.PerformedBy?.FullName ?? string.Empty
                    }).ToList() ?? new()
            };
        }

        private async Task<(int Today, int PendingReview, int Issued, int Rejected)>
            GetOfficeDashboardStatsAsync(int officeId)
        {
            var today = await _appRepo.GetTodayCountAsync(officeId);
            var allApps = await _appRepo.GetByOfficeIdAsync(officeId);

            var pendingReview = allApps.Count(a =>
                a.Status == (int)ApplicationStatus.Submitted ||
                a.Status == (int)ApplicationStatus.UnderReview);

            var issued = allApps.Count(a => a.Status == (int)ApplicationStatus.Issued);
            var rejected = allApps.Count(a => a.Status == (int)ApplicationStatus.Rejected);

            return (today, pendingReview, issued, rejected);
        }

        private async Task<ApplicationTrackResultVM> MapToTrackResultAsync(Models.Application app)
        {
            var appStatus = (ApplicationStatus)app.Status;
            var appServiceType = (ServiceType)app.ServiceType;
            var appType = app.ApplicationType.HasValue
                                 ? (ApplicationType?)app.ApplicationType.Value
                                 : null;

            return new ApplicationTrackResultVM
            {
                ApplicationNumber = app.ApplicationNumber,
                ServiceTypeName = appServiceType.ToArabicName(),
                ApplicationTypeName = appType.HasValue ? appType.Value.ToArabicName() : string.Empty,
                StatusName = appStatus.ToArabicName(),
                StatusColor = appStatus.ToStatusColor(),
                CreatedAt = app.CreatedAt,
                OfficeName = app.Office?.Name ?? string.Empty,
             //   Price = await _pricingService.CalculatePriceAsync(appServiceType, appType),
                Timeline = app.TimelineEntries?
                    .Where(t => !t.IsDeleted)
                    .OrderBy(t => t.Timestamp)
                    .Select(t => new TimelineItemVM
                    {
                        StatusName = ((ApplicationStatus)t.Status).ToArabicName(),
                        Description = t.Description ?? string.Empty,
                        Timestamp = t.Timestamp,
                        PerformedByName = t.PerformedBy?.FullName ?? string.Empty
                    }).ToList() ?? new()
            };
        }
    }
}
