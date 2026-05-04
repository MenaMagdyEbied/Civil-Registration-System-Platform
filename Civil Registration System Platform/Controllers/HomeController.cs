
using Civil_Registration_System_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Civil_Registration_System_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountServices _accountServices;
        private readonly IApplicationService _applicationService;

        // Constructor واحد بيستقبل الاثنين مع بعض
        public HomeController(IAccountServices accountServices, IApplicationService applicationService)
        {
            _accountServices = accountServices;
            _applicationService = applicationService;
        }


        [HttpGet]
        public IActionResult Index()
        {     
            return View();
        }
        [HttpGet]
        public IActionResult Track()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Track(string applicationNumber)
        {
            if (string.IsNullOrWhiteSpace(applicationNumber))
            {
                ModelState.AddModelError(string.Empty, "من فضلك أدخل رقم الطلب.");
                return View();
            }

            var application = await _applicationService.TrackApplicationAsync(applicationNumber);

            if (application == null)
            {
                TempData["Error"] = "لم يتم العثور على طلب بهذا الرقم.";
                return View();
            }

            return View("TrackResult", application);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
