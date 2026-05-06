using Civil_Registration_System_Platform.Enums;

namespace Civil_Registration_System_Platform.ViewModel
{
    public class ServiceStatsVM
    {
        public string ServiceTypeName { get; set; }
        public int TotalApplications { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int IssuedCount { get; set; }
    }

    public class OfficeStatsVM
    {
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string GovernorateName { get; set; }
        public int TotalApplications { get; set; }
        public int PendingCount { get; set; }
        public int IssuedCount { get; set; }
        public int RejectedCount { get; set; }
        public bool IsActive { get; set; }
    }

    public class SuperAdminDashboardVM
    {
        public int TotalApplicationsAllTime { get; set; }
        public int TotalApplicationsToday { get; set; }
        public int TotalPendingReview { get; set; }
        public int TotalIssuedToday { get; set; }
        public int TotalRejectedToday { get; set; }
        public int TotalActiveOffices { get; set; }
        public int TotalUsers { get; set; }
        public List<ApplicationSummaryAdminVM> RecentApplications { get; set; } = new();
        public List<ServiceStatsVM> StatsByService { get; set; } = new();
        public List<OfficeStatsVM> StatsByOffice { get; set; } = new();

        public ApplicationStatus? FilterStatus { get; set; }
        public ServiceType? FilterService { get; set; }
        public int? FilterOfficeId { get; set; }
        public DateTime? FilterDateFrom { get; set; }
        public DateTime? FilterDateTo { get; set; }
    }
}
