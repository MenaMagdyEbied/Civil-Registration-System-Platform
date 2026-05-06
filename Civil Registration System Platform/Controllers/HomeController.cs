
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel.Application;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Civil_Registration_System_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomePageService _homePageService;
        private readonly IApplicationService _applicationService;

        public HomeController(
            IHomePageService homePageService,
            IApplicationService applicationService)
        {
            _homePageService = homePageService;
            _applicationService = applicationService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
            var vm = await _homePageService.GetHomePageDataAsync();
            return View(vm);
        }


        [HttpGet]
        public IActionResult Track(string? id = null)
        {
            var vm = new TrackVM { ApplicationNumber = id ?? string.Empty };
            return View(vm);
            return View(new Civil_Registration_System_Platform.ViewModel.Application.TrackVM());        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Track(string applicationNumber)
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
=======
        public async Task<IActionResult> Track(TrackVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _applicationService.TrackApplicationAsync(model.ApplicationNumber);

            if (result == null)
            {
                TempData["Error"] = "لم يتم العثور على طلب بهذا الرقم";
                ModelState.AddModelError(nameof(model.ApplicationNumber),
                    "لم يتم العثور على طلب بهذا الرقم");
                return View(model);
            }

            model.Result = result;
            return View(model);


            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }








}

