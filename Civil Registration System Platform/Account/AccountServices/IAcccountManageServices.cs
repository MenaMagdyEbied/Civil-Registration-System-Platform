using Civil_Registration_System_Platform.Account.AccountViewModel;

namespace Civil_Registration_System_Platform.Account.AccountServices
{
    public interface IAcccountManageServices
    {
        Task<List<GetAllUserUnConfirmed>> GetUnConfirmedUserAsync();
        Task<string> ConfrimUser(string userId);
        Task<string> RejectUser(string userId, string? messageReject);
        
    }
}
