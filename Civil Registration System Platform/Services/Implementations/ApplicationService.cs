using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _appRepo;
        private readonly IApplicationDocumentRepository _docRepo;
        private readonly ITimelineEntryRepository _timelineRepo;
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IPricingService _pricingService;
        private readonly IFileService _fileService;

        public ApplicationService(
            IApplicationRepository appRepo,
            IApplicationDocumentRepository docRepo,
            ITimelineEntryRepository timelineRepo,
            IAppointmentRepository appointmentRepo,
            IPricingService pricingService,
            IFileService fileService)
        {
            _appRepo = appRepo;
            _docRepo = docRepo;
            _timelineRepo = timelineRepo;
            _appointmentRepo = appointmentRepo;
            _pricingService = pricingService;
            _fileService = fileService;
        }

        public async Task<Application> SubmitApplicationAsync(
            ApplyViewModel model, string userId)
        {
            var appNumber = await _appRepo.GenerateApplicationNumberAsync();

            var price = await _pricingService.CalculatePriceAsync(model.ServiceType, model.ApplicationType);

            var application = new Application
            {
                ApplicationNumber = appNumber,
                ServiceType = (int)model.ServiceType,
                ApplicationType = model.ApplicationType.HasValue? (int)model.ApplicationType.Value: null,
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

            //documents 
            if (model.Documents != null && model.Documents.Any())
            {
                foreach (var file in model.Documents)
                {
                    if (file == null || file.Length == 0) continue;

                    var filePath = await _fileService.UploadDocumentAsync(file, application.ApplicationId);

                    var doc = new ApplicationDocuments
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
                "تم استلام طلبك بنجاح، سيتم مراجعته قريباً",
                userId);

            await _appRepo.SaveChangesAsync();

            return application;
        }

        public async Task<Application?> TrackApplicationAsync(string applicationNumber)
            => await _appRepo.GetByApplicationNumberAsync(applicationNumber);

        public async Task<IEnumerable<Application>> GetUserApplicationsAsync(string userId)
            => await _appRepo.GetByUserIdAsync(userId);

        public async Task<Application?> GetApplicationDetailsAsync(
            int applicationId, string userId)
        {
            var app = await _appRepo.GetWithFullDetailsAsync(applicationId);

            if (app == null || app.UserAccountId != userId)
                return null;

            return app;
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
                ApplicationStatus.UnderReview => "جاري مراجعة طلبك",
                ApplicationStatus.AdditionalInfoRequired => $"مطلوب مستندات إضافية: {note}",
                ApplicationStatus.AppointmentScheduled => "تم تحديد موعدك، تحقق من التفاصيل",
                ApplicationStatus.Approved => "تمت الموافقة على طلبك",
                ApplicationStatus.Rejected => $"تم رفض طلبك. السبب: {note}",
                ApplicationStatus.Issued => "تم إصدار الوثيقة، يمكنك استلامها",
                ApplicationStatus.Cancelled => "تم إلغاء الطلب",
                _ => "تم تحديث حالة طلبك"
            };

            await _timelineRepo.AddEntryAsync( applicationId, newStatus, description, adminId);

            await _appRepo.SaveChangesAsync();
        }

        public async Task ApproveApplicationAsync(int applicationId, string adminId, string? notes)
            => await ChangeStatusAsync(
                applicationId, ApplicationStatus.Approved, adminId, notes);

        public async Task RejectApplicationAsync(int applicationId, string adminId, string reason)
            => await ChangeStatusAsync(
                applicationId, ApplicationStatus.Rejected, adminId, reason);

        public async Task RequestAdditionalInfoAsync(int applicationId, string adminId, string details)
            => await ChangeStatusAsync(
                applicationId, ApplicationStatus.AdditionalInfoRequired, adminId, details);

        public async Task IssueApplicationAsync(int applicationId, string adminId)
            => await ChangeStatusAsync(
                applicationId, ApplicationStatus.Issued, adminId);

        public async Task CancelApplicationAsync(int applicationId, string userId)
        {
            var app = await _appRepo.GetByIdAsync(applicationId);

            if (app == null || app.UserAccountId != userId) return;

            if (app.Status == (int)ApplicationStatus.Approved ||
                app.Status == (int)ApplicationStatus.Issued) return;

            await ChangeStatusAsync(applicationId, ApplicationStatus.Cancelled, userId);
        }

        public async Task<IEnumerable<Application>> GetOfficeApplicationsAsync(
            int officeId, ApplicationStatus? status, ServiceType? serviceType)
            => await _appRepo.GetFilteredAsync(officeId, status, serviceType);

        public async Task<(int Today, int PendingReview, int Issued, int Rejected)>
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
    }
}
