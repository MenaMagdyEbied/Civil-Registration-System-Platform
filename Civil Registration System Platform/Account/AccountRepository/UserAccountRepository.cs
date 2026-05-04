using Civil_Registration_System_Platform.GlobalServices.GlobalInterface;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Civil_Registration_System_Platform.Account.AccountRepository
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserAccount> _userManager; 
        private readonly IUserGlobalServices _userGlobalServices;   
        public UserAccountRepository(AppDbContext context , IUserGlobalServices userGlobalServices, UserManager<UserAccount> userManager)
        {
            _context = context; 
            _userGlobalServices = userGlobalServices;
            _userManager = userManager;
        }

        public async Task<List<UserAccount>> GetUnConfirmedUserAsync()
        {
            UserAccount userLogin  = await _userGlobalServices.GetUser();
            bool isAdmin = await _userGlobalServices.CheckIfIsAdmin();
            if (!isAdmin)
                throw new Exception("you are not admin");
            List<UserAccount> unConfirmedUsers = await _context.UserAccounts
                .Where(u => !u.IsConfirmed && !u.IsRejected && u.OfficeId == userLogin.ManageOfficeId).ToListAsync();

            return unConfirmedUsers;
        }
        public async Task<UserAccount> GetById(string userId)
        {
            UserAccount userLogin = await _userGlobalServices.GetUser();
            bool isAdmin = await _userGlobalServices.CheckIfIsAdmin();
            if (!isAdmin)
                throw new Exception("you are not admin");
            UserAccount? userAccount = await _context.UserAccounts.Where(u=>u.OfficeId == userLogin.ManageOfficeId).SingleOrDefaultAsync(u => u.Id == userId);    

            return userAccount; 
        }
        public async Task<UserAccount> SaveUser(UserAccount userAccount)
        {

            var result = await _userManager.UpdateAsync(userAccount);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return userAccount;
        }

        public async Task<UserAccount> GetMyAccount()
        {
            UserAccount userLogin = await _userGlobalServices.GetUser();
            return userLogin;
        }
    }
}
