using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Civil_Registration_System_Platform.Controllers
{
   
    [Authorize(Roles = "User")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IApplicationService _applicationService;
        private readonly UserManager<UserAccount> _userManager;

        public DashboardController(
            IDashboardService dashboardService,
            IApplicationService applicationService,
            UserManager<UserAccount> userManager)
        {
            _dashboardService = dashboardService;
            _applicationService = applicationService;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var vm = await _dashboardService.GetCitizenDashboardAsync(userId);
            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> MyApplications()
        {
            var userId = _userManager.GetUserId(User);
            var applications = await _applicationService.GetUserApplicationsAsync(userId);
            return View(applications);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);
            var details = await _applicationService.GetApplicationDetailsAsync(id, userId);

            if (details == null)
            {
                TempData["Error"] = "لم يتم العثور على الطلب أو ليس لديك صلاحية لعرضه";
                return RedirectToAction(nameof(MyApplications));
            }

            return View(details);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = _userManager.GetUserId(User);

            try
            {
                await _applicationService.CancelApplicationAsync(id, userId);
                TempData["Success"] = "تم إلغاء الطلب بنجاح";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(MyApplications));
        }
    }
}
