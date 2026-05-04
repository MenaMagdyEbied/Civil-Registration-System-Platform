using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Civil_Registration_System_Platform.Controllers
{
    [Authorize(Roles = "superadmin")]
    public class SuperAdminController : Controller
    {
        private readonly UserManager<UserAccount> _userManager;

        public SuperAdminController(UserManager<UserAccount> userManager)
        {
            _userManager = userManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");

            var viewModel = new SuperAdminDashboardViewModel
            {
                Admins = admins.ToList(),
                TotalAdmins = admins.Count
            };

            return View(viewModel);
        }





















    }
}
