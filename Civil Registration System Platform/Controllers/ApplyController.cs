using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.GlobalServices.GlobalInterface;
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Civil_Registration_System_Platform.Controllers
{
    /// <summary>
    /// تقديم طلب جديد — apply.html
    /// GET: عرض بيانات الخدمة (أنواع / أسعار / مستندات / محافظات)
    /// POST: إرسال الطلب (المواطن لازم يكون معتمد IsConfirmed = true)
    /// </summary>
    [Authorize(Roles = "User")]
    public class ApplyController : Controller
    {
        private readonly IApplyFormService _applyFormService;
        private readonly IApplicationService _applicationService;
        private readonly IOfficeServices _officeServices;
        private readonly IUserGlobalServices _userGlobalServices;
        private readonly UserManager<UserAccount> _userManager;

        public ApplyController(
            IApplyFormService applyFormService,
            IApplicationService applicationService,
            IOfficeServices officeServices,
            IUserGlobalServices userGlobalServices,
            UserManager<UserAccount> userManager)
        {
            _applyFormService = applyFormService;
            _applicationService = applicationService;
            _officeServices = officeServices;
            _userGlobalServices = userGlobalServices;
            _userManager = userManager;
        }

        // ───────── Index — GET /Apply?serviceType=6 ─────────

        [HttpGet]
        public async Task<IActionResult> Index(ServiceType serviceType)
        {
            // التحقق من اعتماد الحساب — حتى لو دخل الصفحة بنخبره يستنى
            var user = await _userGlobalServices.GetUser();
            if (!user.IsConfirmed)
            {
                TempData["Error"] = user.IsRejected
                    ? $"تم رفض حسابك: {user.RejectionMessage}"
                    : "حسابك بانتظار اعتماد الأدمن — لا يمكنك التقديم بعد";
                return RedirectToAction("Index", "Dashboard");
            }

            var formData = await _applyFormService.GetApplyFormDataAsync(serviceType);

            if (formData == null)
            {
                TempData["Error"] = "الخدمة المطلوبة غير متاحة حالياً";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.FormData = formData;
            var model = new ApplyViewModel { ServiceType = serviceType };
            return View(model);
        }

        // ───────── Submit — POST /Apply/Submit ─────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ApplyViewModel model)
        {
            // إعادة التحقق من اعتماد الحساب — defense in depth
            var user = await _userGlobalServices.GetUser();
            if (!user.IsConfirmed)
            {
                TempData["Error"] = "حسابك بانتظار الاعتماد — لا يمكنك التقديم";
                return RedirectToAction("Index", "Dashboard");
            }

            if (!ModelState.IsValid)
            {
                var formData = await _applyFormService.GetApplyFormDataAsync(model.ServiceType);
                ViewBag.FormData = formData;
                return View("Index", model);
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                var appNumber = await _applicationService.SubmitApplicationAsync(model, userId);

                TempData["Success"] = $"تم تقديم طلبك بنجاح. رقم الطلب: {appNumber}";
                return RedirectToAction("Confirmation", new { applicationNumber = appNumber });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var formData = await _applyFormService.GetApplyFormDataAsync(model.ServiceType);
                ViewBag.FormData = formData;
                return View("Index", model);
            }
        }

        // ───────── Confirmation — GET /Apply/Confirmation?applicationNumber=CRS-2025-0042 ─────────

        [HttpGet]
        public IActionResult Confirmation(string applicationNumber)
        {
            if (string.IsNullOrWhiteSpace(applicationNumber))
                return RedirectToAction("Index", "Home");

            ViewBag.ApplicationNumber = applicationNumber;
            return View();
        }

        // ───────── GetOffices — AJAX — GET /Apply/GetOffices?governorateId=1 ─────────

        [HttpGet]
        public async Task<IActionResult> GetOffices(int governorateId)
        {
            var offices = await _officeServices.GetByGovernorateIdAsync(governorateId);
            return Json(offices);
        }

        // ───────── RespondToAdditionalInfo — POST /Apply/RespondToAdditionalInfo ─────────
        // المواطن يرفع مستندات إضافية ردًا على طلب الأدمن (الحالة AdditionalInfoRequired)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RespondToAdditionalInfo(
            int applicationId,
            List<IFormFile>? additionalDocs,
            string? note)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var result = await _applicationService.RespondToAdditionalInfoAsync(
                    applicationId, userId, additionalDocs, note);

                TempData["Success"] = result;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Details", "Dashboard", new { id = applicationId });
        }
    }
}
