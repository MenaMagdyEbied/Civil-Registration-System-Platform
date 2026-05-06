using Civil_Registration_System_Platform.Account.AccountRepository;
using Civil_Registration_System_Platform.Account.AccountServices;
using Civil_Registration_System_Platform.Account.AccountViewModel;
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    // الأدمن بيدير موظفي مكتبه
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private readonly IUserAccountRepository _userRepo;
        private readonly IAccountServices _accountServices;

        public EmployeeManagementService(
            IUserAccountRepository userRepo,
            IAccountServices accountServices)
        {
            _userRepo = userRepo;
            _accountServices = accountServices;
        }

        public async Task<List<EmployeeListItemVM>> GetEmployeesByOfficeAsync(int officeId)
            => await GetEmployeesByOfficesAsync(new[] { officeId });

        public async Task<List<EmployeeListItemVM>> GetEmployeesByOfficesAsync(IEnumerable<int> officeIds)
        {
            var employees = await _userRepo.GetEmployeesByOfficesAsync(officeIds);

            return employees.Select(e => new EmployeeListItemVM
            {
                EmployeeId = e.Id,
                FullName = e.FullName,
                Email = e.Email ?? string.Empty,
                EGPhoneNumber = e.EGPhoneNumber,
                NationalID = e.NationalID,
                CardImagePath = e.CardImagePath,
                CreatedAt = e.CreatedAt,
                OfficeId = e.OfficeId,
                OfficeName = e.Office?.Name,
                GovernorateName = e.Governorate?.Name,
                IsActive = e.IsConfirmed && !e.IsRejected
            }).ToList();
        }

        public async Task<EmployeeListItemVM?> GetEmployeeAsync(string employeeId, int officeId)
            => await GetEmployeeAsync(employeeId, new[] { officeId });

        public async Task<EmployeeListItemVM?> GetEmployeeAsync(string employeeId, IEnumerable<int> officeIds)
        {
            var e = await _userRepo.GetEmployeeByIdAsync(employeeId, officeIds);
            if (e == null) return null;

            return new EmployeeListItemVM
            {
                EmployeeId = e.Id,
                FullName = e.FullName,
                Email = e.Email ?? string.Empty,
                EGPhoneNumber = e.EGPhoneNumber,
                NationalID = e.NationalID,
                CardImagePath = e.CardImagePath,
                CreatedAt = e.CreatedAt,
                OfficeId = e.OfficeId,
                OfficeName = e.Office?.Name,
                GovernorateName = e.Governorate?.Name,
                IsActive = e.IsConfirmed && !e.IsRejected
            };
        }

        public async Task<string> CreateEmployeeAsync(RegisterAdminOrEmployeeViewModel model)
            => await _accountServices.RegisterEmployeeAsync(model);

        public async Task<string> ToggleEmployeeActiveAsync(string employeeId, int officeId)
            => await ToggleEmployeeActiveAsync(employeeId, new[] { officeId });

        public async Task<string> ToggleEmployeeActiveAsync(string employeeId, IEnumerable<int> officeIds)
        {
            var employee = await _userRepo.GetEmployeeByIdAsync(employeeId, officeIds);
            if (employee == null)
                throw new Exception("لم يتم العثور على الموظف داخل هذا المكتب");

            if (employee.IsRejected)
            {
                employee.IsRejected = false;
                employee.RejectionMessage = null;
                employee.IsConfirmed = true;
            }
            else
            {
                employee.IsRejected = true;
                employee.IsConfirmed = false;
                employee.RejectionMessage = "تم تعطيل الحساب بواسطة الأدمن";
            }

            await _userRepo.SaveUser(employee);
            return employee.IsRejected ? "تم تعطيل الموظف" : "تم تفعيل الموظف";
        }
    }
}
