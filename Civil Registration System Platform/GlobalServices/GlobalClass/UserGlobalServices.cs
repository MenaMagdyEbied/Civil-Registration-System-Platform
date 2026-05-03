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
            string? userName = _userManager.GetUserId(user);
            if (userName == null) throw new Exception("Not found this User");
            UserAccount? userLogin = await _userManager.FindByNameAsync(userName);
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
    }
}
