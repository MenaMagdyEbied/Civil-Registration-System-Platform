namespace Civil_Registration_System_Platform.GlobalServices.GlobalInterface
{
    public interface IUserGlobalServices
    {
        Task<UserAccount> GetUser();
        Task<bool> CheckIfIsAdmin();
    }
}
