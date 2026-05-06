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
        private readonly IPricingService _pricingService;
        private readonly IFileService _fileService;

        public ApplicationService(
            IApplicationRepository appRepo,
            IApplicationDocumentRepository docRepo,
            ITimelineEntryRepository timelineRepo,
            IPricingService pricingService,
            IFileService fileService)
        {
            _appRepo = appRepo;
            _docRepo = docRepo;
            _timelineRepo = timelineRepo;
            _pricingService = pricingService;
            _fileService = fileService;
        }

        // ─── Submit 

        public async Task<string> SubmitApplicationAsync(ApplyViewModel model, string userId)
        {
            var appNumber = await _appRepo.GenerateApplicationNumberAsync();

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

                    await _docRepo.AddAsync(new Models.ApplicationDocuments
                    {
                        ApplicationId = application.ApplicationId,
                        Name = file.FileName,
                        DocumentPath = filePath,
                        UploadedAt = DateTime.UtcNow,
                        UserAccountId = userId,
                        IsDeleted = false
                    });
                }
            }

            await _timelineRepo.AddEntryAsync(
                application.ApplicationId,
                ApplicationStatus.Submitted,
                "تم استلام طلبك بنجاح، سيتم مراجعته قريباً",
                userId);

            await _appRepo.SaveChangesAsync();

            return appNumber;
        }

        // ─── Track 

        public async Task<ApplicationTrackResultVM?> TrackApplicationAsync(string applicationNumber)
        {
            var app = await _appRepo.GetByApplicationNumberAsync(applicationNumber);
            if (app == null) return null;

            return await MapToTrackResultAsync(app);
        }

        // ─── قائمة طلبات المواطن 

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

        // ───  المواطن يشوف طلبه 

        public async Task<ApplicationTrackResultVM?> GetApplicationDetailsAsync(
            int applicationId, string userId)
        {
            var app = await _appRepo.GetWithFullDetailsAsync(applicationId);

            if (app == null || app.UserAccountId != userId)
                return null;

            return await MapToTrackResultAsync(app);
        }

        public async Task CancelApplicationAsync(int applicationId, string userId)
        {
            var app = await _appRepo.GetByIdAsync(applicationId);

            if (app == null || app.UserAccountId != userId) return;

            // مينفعش يلغي لو اتوافق أو اتصدر
            if (app.Status == (int)ApplicationStatus.Approved ||
                app.Status == (int)ApplicationStatus.Issued) return;

            app.Status = (int)ApplicationStatus.Cancelled;
            app.UpdatedAt = DateTime.UtcNow;

            await _appRepo.UpdateAsync(app);

            await _timelineRepo.AddEntryAsync(
                applicationId,
                ApplicationStatus.Cancelled,
                "تم إلغاء الطلب من قِبل المتقدم",
                userId);

            await _appRepo.SaveChangesAsync();
        }

        //  المواطن يرد على طلب الأدمن للمستندات الإضافية 
        public async Task<string> RespondToAdditionalInfoAsync(
            int applicationId,
            string userId,
            List<IFormFile>? additionalDocs,
            string? note)
        {
            var app = await _appRepo.GetByIdAsync(applicationId);

            if (app == null || app.UserAccountId != userId)
                throw new Exception("الطلب غير موجود أو ليس لديك صلاحية");

            if (app.Status != (int)ApplicationStatus.AdditionalInfoRequired)
                throw new Exception("لا يمكن الرد — حالة الطلب لا تتطلب مستندات إضافية");

            // رفع المستندات الجديدة
            var uploadedCount = 0;
            if (additionalDocs != null && additionalDocs.Count > 0)
            {
                foreach (var file in additionalDocs)
                {
                    if (file == null || file.Length == 0) continue;

                    var filePath = await _fileService.UploadDocumentAsync(file, app.ApplicationId);

                    await _docRepo.AddAsync(new Models.ApplicationDocuments
                    {
                        ApplicationId = app.ApplicationId,
                        Name          = file.FileName,
                        DocumentPath  = filePath,
                        UploadedAt    = DateTime.UtcNow,
                        Description   = "مستند إضافي بناءً على طلب الأدمن",
                        UserAccountId = userId,
                        IsDeleted     = false
                    });

                    uploadedCount++;
                }
            }

            // ترجع الحالة لـ UnderReview علشان الأدمن يراجع تاني
            app.Status = (int)ApplicationStatus.UnderReview;
            app.UpdatedAt = DateTime.UtcNow;
            await _appRepo.UpdateAsync(app);

            var description = uploadedCount > 0
                ? $"تم رفع {uploadedCount} مستند إضافي ردًا على طلب الأدمن"
                : "تم تحديث الطلب ردًا على طلب الأدمن";

            if (!string.IsNullOrWhiteSpace(note))
                description += $" — ملاحظة المتقدم: {note}";

            await _timelineRepo.AddEntryAsync(
                applicationId,
                ApplicationStatus.UnderReview,
                description,
                userId);

            await _appRepo.SaveChangesAsync();

            return uploadedCount > 0
                ? $"تم رفع {uploadedCount} مستند بنجاح وأعيد الطلب للمراجعة"
                : "تم تحديث الطلب وأعيد للمراجعة";
        }

        // ─── Private helper 

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
                OfficeName  = app.Office?.Name ?? string.Empty,
                Price = await _pricingService.CalculatePriceAsync(appServiceType, appType),

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
