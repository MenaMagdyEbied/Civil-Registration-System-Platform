using Civil_Registration_System_Platform.Account.AccountViewModel;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Interfaces
{
    // إدارة الموظفين — الأدمن لمكتبه فقط
    
    public interface IEmployeeManagementService
    {
        Task<List<EmployeeListItemVM>> GetEmployeesByOfficeAsync(int officeId);
        Task<List<EmployeeListItemVM>> GetEmployeesByOfficesAsync(IEnumerable<int> officeIds);
        Task<EmployeeListItemVM?> GetEmployeeAsync(string employeeId, int officeId);
        Task<EmployeeListItemVM?> GetEmployeeAsync(string employeeId, IEnumerable<int> officeIds);

        Task<string> CreateEmployeeAsync(RegisterAdminOrEmployeeViewModel model);

        Task<string> ToggleEmployeeActiveAsync(string employeeId, int officeId);
        Task<string> ToggleEmployeeActiveAsync(string employeeId, IEnumerable<int> officeIds);
    }
}
