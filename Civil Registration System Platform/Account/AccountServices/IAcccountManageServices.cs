using Civil_Registration_System_Platform.Account.AccountViewModel;
using Civil_Registration_System_Platform.ViewModel;

namespace Civil_Registration_System_Platform.Account.AccountServices
{
    public interface IAcccountManageServices
    {
        Task<List<GetAllUserUnConfirmed>> GetUnConfirmedUserAsync();
        Task<string> ConfrimUser(string userId);
        Task<string> RejectUser(string userId, string? messageReject);
        Task<List<OfficeUserListItemVM>> GetConfirmedUsersByOfficeAsync(int officeId);

        Task<List<OfficeUserListItemVM>> GetRejectedUsersByOfficeAsync(int officeId);
        Task<List<OfficeUserListItemVM>> GetAllUsersByOfficeAsync(int officeId);
    }
}
