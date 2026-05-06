using Civil_Registration_System_Platform.GlobalServices.GlobalInterface;
using Microsoft.AspNetCore.Identity;

namespace Civil_Registration_System_Platform.GlobalServices.GlobalClass
{
    public sealed class UserGlobalServices : IUserGlobalServices
    {
        private  readonly UserManager<UserAccount> _userManager;
        private  readonly IHttpContextAccessor _httpContextAccessor;

        public UserGlobalServices(IHttpContextAccessor httpContextAccessor, UserManager<UserAccount> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<UserAccount> GetUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) throw new Exception("Not found this User");
            string? userId = _userManager.GetUserId(user);
            if (userId == null) throw new Exception("Not found this User");
            UserAccount? userLogin = await _userManager.FindByIdAsync(userId);
            if (userLogin == null) throw new Exception("Not found this User");

            return userLogin;
        }

        public async Task<bool> CheckIfIsAdmin()
        {
            UserAccount userLogin = await GetUser();
            if (!await _userManager.IsInRoleAsync(userLogin, "Admin"))
                throw new Exception("you are not an admin");
            return true;
        }

        public async Task<bool> CheckIfCanReviewAccounts()
        {
            UserAccount userLogin = await GetUser();
            var isAdmin = await _userManager.IsInRoleAsync(userLogin, "Admin");
            var isReviewer = await _userManager.IsInRoleAsync(userLogin, "AccountReviewer");
            if (!isAdmin && !isReviewer)
                throw new Exception("you cannot review citizen accounts");
            return true;
        }

        public async Task<bool> IsAccountReviewer()
        {
            UserAccount userLogin = await GetUser();
            return await _userManager.IsInRoleAsync(userLogin, "AccountReviewer");
        }
    }
}
