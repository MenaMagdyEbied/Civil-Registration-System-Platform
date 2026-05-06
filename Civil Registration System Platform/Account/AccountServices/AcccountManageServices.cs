using Civil_Registration_System_Platform.Account.AccountRepository;
using Civil_Registration_System_Platform.Account.AccountViewModel;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Account.AccountServices
{
    public class AcccountManageServices : IAcccountManageServices
    {
        private readonly IUserAccountRepository _userAccountRepository;
        public AcccountManageServices(IUserAccountRepository userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }

        public async Task<List<GetAllUserUnConfirmed>> GetUnConfirmedUserAsync()
        {
            List<UserAccount> userAccounts = await _userAccountRepository.GetUnConfirmedUserAsync();
            List<GetAllUserUnConfirmed> getAllUserUnConfirmeds = new List<GetAllUserUnConfirmed>();
            foreach (var userAccount in userAccounts)
            {
                getAllUserUnConfirmeds.Add(new GetAllUserUnConfirmed
                {
                    UserId = userAccount.Id,
                    FullName = userAccount.FullName,
                    EGPhoneNumber = userAccount.EGPhoneNumber,
                    NationalID = userAccount.NationalID,
                    Email = userAccount.Email,  
                    CardImagePath = userAccount.CardImagePath,  
                    Gender = userAccount.Gender,
                    MaritalStatus = userAccount.MaritalStatus,      
                    GovernorateName = userAccount.Governorate.Name,
                    OfficeName = userAccount.Office.Name ,
                    CreatedAt = userAccount.CreatedAt
                });
            }
            return getAllUserUnConfirmeds;      
        }

        public async Task<string> ConfrimUser(string userId)
        {
            UserAccount? userAccount =await _userAccountRepository.GetById(userId);
            if (userAccount == null)
                throw new Exception("لم يتم العثور على المستخدم");

            userAccount.IsConfirmed = true;

            await _userAccountRepository.SaveUser(userAccount);

            return "تم اعتماد الحساب بنجاح";    
        }

        public async Task<string> RejectUser(string userId , string? messageReject)
        {
            UserAccount? userAccount = await _userAccountRepository.GetById(userId);
            if (userAccount == null)
                throw new Exception("لم يتم العثور على المستخدم");

            userAccount.IsConfirmed = false;
            userAccount.IsRejected = true;

            userAccount.RejectionMessage = messageReject ?? "تم رفض طلبك.";
            await _userAccountRepository.SaveUser(userAccount);

            return "تم رفض الحساب بنجاح";
        }


        public async Task<List<OfficeUserListItemVM>> GetConfirmedUsersByOfficeAsync(int officeId)
        {
            var users = await _userAccountRepository.GetConfirmedUsersByOfficeAsync(officeId);
            return users.Select(MapToVM).ToList();
        }

        public async Task<List<OfficeUserListItemVM>> GetRejectedUsersByOfficeAsync(int officeId)
        {
            var users = await _userAccountRepository.GetRejectedUsersByOfficeAsync(officeId);
            return users.Select(MapToVM).ToList();
        }

        public async Task<List<OfficeUserListItemVM>> GetAllUsersByOfficeAsync(int officeId)
        {
            var users = await _userAccountRepository.GetAllUsersByOfficeAsync(officeId);
            return users.Select(MapToVM).ToList();
        }

        // ───── helper ─────
        private static OfficeUserListItemVM MapToVM(UserAccount u) => new()
        {
            UserId           = u.Id,
            FullName         = u.FullName,
            Email            = u.Email ?? string.Empty,
            EGPhoneNumber    = u.EGPhoneNumber,
            NationalID       = u.NationalID,
            CardImagePath    = u.CardImagePath,
            Gender           = u.Gender,
            MaritalStatus    = u.MaritalStatus,
            CreatedAt        = u.CreatedAt,
            GovernorateName  = u.Governorate?.Name,
            OfficeName       = u.Office?.Name,
            IsConfirmed      = u.IsConfirmed,
            IsRejected       = u.IsRejected,
            RejectionMessage = u.RejectionMessage
        };
    }
}
