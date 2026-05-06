using Civil_Registration_System_Platform.Account.AccountViewModel;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Interfaces
{
    // إدارة الأدمن — السوبر أدمن فقط
    public interface IAdminManagementService
    {
        Task<List<AdminListItemVM>> GetAllAdminsAsync();
        Task<AdminListItemVM?> GetAdminAsync(string adminId);
        Task<string> CreateAdminAsync(RegisterAdminOrEmployeeViewModel model);

        Task<string> ToggleAdminActiveAsync(string adminId);
    }
}
