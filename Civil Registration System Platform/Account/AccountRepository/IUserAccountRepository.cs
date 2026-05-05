namespace Civil_Registration_System_Platform.Account.AccountRepository
{
    public interface IUserAccountRepository
    {
        Task<List<UserAccount>> GetUnConfirmedUserAsync();
        Task<UserAccount> GetById(string userId);   
        Task<UserAccount> SaveUser(UserAccount userAccount);
        Task<UserAccount> GetMyAccount();

    }
}
