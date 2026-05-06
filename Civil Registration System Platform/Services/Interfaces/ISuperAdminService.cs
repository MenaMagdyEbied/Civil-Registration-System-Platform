using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Interfaces
{
    // داشبورد السوبر أدمن — إحصائيات كل المكاتب
    public interface ISuperAdminService
    {
        Task<SuperAdminDashboardVM> GetDashboardAsync(
            ApplicationStatus? filterStatus,
            ServiceType? filterService,
            int? filterOfficeId,
            DateTime? dateFrom,
            DateTime? dateTo);
    }
}
