
using Civil_Registration_System_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Civil_Registration_System_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApplicationService _applicationService;

        public HomeController(IApplicationService applicationService)
        {
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
            return View(new Civil_Registration_System_Platform.ViewModel.Application.TrackVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Track(Civil_Registration_System_Platform.ViewModel.Application.TrackVM model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.ApplicationNumber))
            {
                ModelState.AddModelError(string.Empty, "من فضلك أدخل رقم الطلب.");
                return View(model ?? new Civil_Registration_System_Platform.ViewModel.Application.TrackVM());
            }

            try
            {
                model.Result = await _applicationService.TrackApplicationAsync(model.ApplicationNumber);
                if (model.Result == null)
                {
                    TempData["ErrorMessage"] = "لم يتم العثور على طلب بهذا الرقم.";
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء البحث. حاول مرة أخرى.";
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
      
    }
}
