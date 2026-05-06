using Civil_Registration_System_Platform.Account.AccountRepository;
using Civil_Registration_System_Platform.Account.AccountServices;
using Civil_Registration_System_Platform.Account.AccountViewModel;
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    //السوبر أدمن بيدير الأدمن
    public class AdminManagementService : IAdminManagementService
    {
        private readonly IUserAccountRepository _userRepo;
        private readonly IAccountServices _accountServices;

        public AdminManagementService(
            IUserAccountRepository userRepo,
            IAccountServices accountServices)
        {
            _userRepo = userRepo;
            _accountServices = accountServices;
        }

        public async Task<List<AdminListItemVM>> GetAllAdminsAsync()
        {
            var admins = await _userRepo.GetAdminsAsync();

            return admins.Select(a => new AdminListItemVM
            {
                AdminId = a.Id,
                FullName = a.FullName,
                Email = a.Email ?? string.Empty,
                EGPhoneNumber = a.EGPhoneNumber,
                NationalID = a.NationalID,
                CardImagePath = a.CardImagePath,
                CreatedAt = a.CreatedAt,
                OfficeId = a.OfficeId,
                OfficeName = a.Office?.Name,
                ManageOfficeId = a.ManageOfficeId,
                ManageOfficeName = a.ManageOffice?.Name,
                GovernorateName = a.Governorate?.Name,
                IsActive = a.IsConfirmed && !a.IsRejected
            }).ToList();
        }

        public async Task<AdminListItemVM?> GetAdminAsync(string adminId)
        {
            var a = await _userRepo.GetAdminByIdAsync(adminId);
            if (a == null) return null;

            return new AdminListItemVM
            {
                AdminId = a.Id,
                FullName = a.FullName,
                Email = a.Email ?? string.Empty,
                EGPhoneNumber = a.EGPhoneNumber,
                NationalID = a.NationalID,
                CardImagePath = a.CardImagePath,
                CreatedAt = a.CreatedAt,
                OfficeId = a.OfficeId,
                OfficeName = a.Office?.Name,
                ManageOfficeId = a.ManageOfficeId,
                ManageOfficeName = a.ManageOffice?.Name,
                GovernorateName = a.Governorate?.Name,
                IsActive = a.IsConfirmed && !a.IsRejected
            };
        }

        public async Task<string> CreateAdminAsync(RegisterAdminOrEmployeeViewModel model)
            => await _accountServices.RegisterAdminAsync(model);

        public async Task<string> ToggleAdminActiveAsync(string adminId)
        {
            var admin = await _userRepo.GetAdminByIdAsync(adminId);
            if (admin == null)
                throw new Exception("لم يتم العثور على الأدمن");

            if (admin.IsRejected)
            {
                admin.IsRejected = false;
                admin.RejectionMessage = null;
                admin.IsConfirmed = true;
            }
            else
            {
                admin.IsRejected = true;
                admin.IsConfirmed = false;
                admin.RejectionMessage = "تم تعطيل الحساب بواسطة السوبر أدمن";
            }

            await _userRepo.SaveUser(admin);
            return admin.IsRejected ? "تم تعطيل الأدمن" : "تم تفعيل الأدمن";
        }
    }
}
