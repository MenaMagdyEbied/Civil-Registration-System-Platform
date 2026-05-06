using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Interfaces
{
    // داشبورد المواطن — إحصائياته + طلباته + مواعيده القادمة
    public interface IDashboardService
    {
        Task<DashboardVM> GetCitizenDashboardAsync(string userId);
    }
}
