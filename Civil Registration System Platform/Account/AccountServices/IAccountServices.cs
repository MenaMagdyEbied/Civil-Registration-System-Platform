using Civil_Registration_System_Platform.Account.AccountViewModel;

namespace Civil_Registration_System_Platform.Account.AccountServices
{
    public interface IAccountServices
    {
        Task<string> RegisterUserAsync(RegisterViewModel registerViewModel);
        Task<string> RegisterEmployeeAsync(RegisterAdminOrEmployeeViewModel registerEmployeeViewModel);
        Task<string> RegisterAdminAsync(RegisterAdminOrEmployeeViewModel registerEmployeeViewModel);
        Task<string> LoginUserAsync(LoginViewModel loginViewModel);


        //new must user created account and login to get his account information
        Task<GetMyAccount> GetMyAccount(); 
        Task<string> EditMyAccount(UserAccountEdit userAccountEdit); 
    }
}
