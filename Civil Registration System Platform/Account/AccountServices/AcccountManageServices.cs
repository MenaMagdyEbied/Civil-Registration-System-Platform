using Civil_Registration_System_Platform.Account.AccountRepository;
using Civil_Registration_System_Platform.Account.AccountViewModel;

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
                    GovernorateId = (int)userAccount.GovernorateId,
                    OfficeId = (int)userAccount.OfficeId ,
                    CreatedAt = userAccount.CreatedAt
                });
            }
            return getAllUserUnConfirmeds;      
        }

        public async Task<string> ConfrimUser(string userId)
        {
            UserAccount? userAccount =await _userAccountRepository.GetById(userId);
            if (userAccount == null)
                throw new Exception("User not found");

            userAccount.IsConfirmed = true;

            await _userAccountRepository.SaveUser(userAccount);

            return "Confirmed successfully";    
        }

        public async Task<string> RejectUser(string userId , string? messageReject)
        {
            UserAccount? userAccount = await _userAccountRepository.GetById(userId);
            if (userAccount == null)
                throw new Exception("User not found");

            userAccount.IsConfirmed = false;
            userAccount.IsRejected = true;

            userAccount.RejectionMessage = messageReject ?? "Your request has been rejected.";
            await _userAccountRepository.SaveUser(userAccount);

            return "Rejected successfully";
        }
    }
}
