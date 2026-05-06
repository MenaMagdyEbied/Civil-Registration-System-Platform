namespace Civil_Registration_System_Platform.Account.AccountRepository
{
    public interface IUserAccountRepository
    {
        Task<List<UserAccount>> GetUnConfirmedUserAsync();
        Task<UserAccount> GetById(string userId);
        Task<UserAccount> SaveUser(UserAccount userAccount);
        Task<UserAccount> GetMyAccount();
        Task<List<UserAccount>> GetAdminsAsync();

        Task<UserAccount?> GetAdminByIdAsync(string adminId);

        Task<List<UserAccount>> GetEmployeesByOfficeAsync(int officeId);
        Task<List<UserAccount>> GetEmployeesByOfficesAsync(IEnumerable<int> officeIds);

        Task<UserAccount?> GetEmployeeByIdAsync(string employeeId, int officeId);
        Task<UserAccount?> GetEmployeeByIdAsync(string employeeId, IEnumerable<int> officeIds);

        Task<List<UserAccount>> GetConfirmedUsersByOfficeAsync(int officeId);

        Task<List<UserAccount>> GetRejectedUsersByOfficeAsync(int officeId);

        Task<List<UserAccount>> GetAllUsersByOfficeAsync(int officeId);
    }
}
