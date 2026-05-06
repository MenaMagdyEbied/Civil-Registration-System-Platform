using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.GlobalServices.GlobalInterface;
using Civil_Registration_System_Platform.Services.Interfaces;
using Civil_Registration_System_Platform.ViewModel.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Civil_Registration_System_Platform.Controllers
{
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

       

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SelectService() => View();


        [HttpGet]
        public async Task<IActionResult> Index(ServiceType serviceType)
        {
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ApplyViewModel model)
        {
            var user = await _userGlobalServices.GetUser();
            if (!user.IsConfirmed)
            {
                TempData["Error"] = "حسابك بانتظار الاعتماد — لا يمكنك التقديم";
                return RedirectToAction("Index", "Dashboard");
            }

            ValidateApplicantMatchesAccount(model, user);

            if (!ModelState.IsValid)
            {
                var formData = await _applyFormService.GetApplyFormDataAsync(model.ServiceType);
                ViewBag.FormData = formData;
                return View("Index", model);
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrWhiteSpace(userId))
                    throw new Exception("تعذر تحديد المستخدم الحالي.");

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateApplicant(ApplyViewModel model)
        {
            var user = await _userGlobalServices.GetUser();
            if (!user.IsConfirmed)
            {
                return Json(new
                {
                    isValid = false,
                    errors = new[] { "حسابك بانتظار الاعتماد، لا يمكنك التقديم حالياً." }
                });
            }

            var errors = GetApplicantMatchErrors(model, user);
            return Json(new { isValid = !errors.Any(), errors });
        }


        [HttpGet]
        public IActionResult Confirmation(string applicationNumber)
        {
            if (string.IsNullOrWhiteSpace(applicationNumber))
                return RedirectToAction("Index", "Home");

            ViewBag.ApplicationNumber = applicationNumber;
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetOffices(int governorateId)
        {
            var offices = await _officeServices.GetByGovernorateIdAsync(governorateId);
            return Json(offices);
        }



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

        private void ValidateApplicantMatchesAccount(ApplyViewModel model, UserAccount user)
        {
            foreach (var error in GetApplicantMatchErrors(model, user))
                ModelState.AddModelError(error.Key, error.Value);
        }

        private static Dictionary<string, string> GetApplicantMatchErrors(ApplyViewModel model, UserAccount user)
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(model.FullName))
                errors[nameof(model.FullName)] = "الاسم مطلوب.";
            else if (!MatchesText(model.FullName, user.FullName))
                errors[nameof(model.FullName)] = "الاسم يجب أن يطابق الاسم المسجل في حسابك.";

            if (string.IsNullOrWhiteSpace(model.NationalId))
                errors[nameof(model.NationalId)] = "الرقم القومي مطلوب.";
            else if (!MatchesDigits(model.NationalId, user.NationalID))
                errors[nameof(model.NationalId)] = "الرقم القومي يجب أن يطابق الرقم المسجل في حسابك.";

            if (string.IsNullOrWhiteSpace(model.Phone))
                errors[nameof(model.Phone)] = "رقم الموبايل مطلوب.";
            else if (!MatchesDigits(model.Phone, user.EGPhoneNumber))
                errors[nameof(model.Phone)] = "رقم الموبايل يجب أن يطابق الرقم المسجل في حسابك.";

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                if (string.IsNullOrWhiteSpace(model.Email))
                    errors[nameof(model.Email)] = "البريد الإلكتروني مطلوب.";
                else if (!MatchesEmail(model.Email, user.Email))
                    errors[nameof(model.Email)] = "البريد الإلكتروني يجب أن يطابق البريد المسجل في حسابك.";
            }

            return errors;
        }

        private static bool MatchesText(string? submitted, string? registered)
        {
            if (submitted == null || registered == null) return false;
            
            var a = NormalizeWhitespace(submitted).ToLower();
            var b = NormalizeWhitespace(registered).ToLower();
            
            if (a == b) return true;

          
            return a.Contains(b) || b.Contains(a);
        }

        private static bool MatchesDigits(string? submitted, string? registered)
        {
            if (submitted == null || registered == null) return false;
            var a = DigitsOnly(NormalizeArabicDigits(submitted.Trim()));
            var b = DigitsOnly(NormalizeArabicDigits(registered.Trim()));
            return a.Length > 0 && a == b;
        }

        private static bool MatchesEmail(string? submitted, string? registered)
        {
            if (submitted == null || registered == null) return false;
            return string.Equals(submitted.Trim(), registered.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeWhitespace(string s)
            => System.Text.RegularExpressions.Regex.Replace(s.Trim(), @"\s+", " ");

        private static string DigitsOnly(string s)
            => System.Text.RegularExpressions.Regex.Replace(s, @"\D", "");

        private static string NormalizeArabicDigits(string s)
        {
            var arabic  = "٠١٢٣٤٥٦٧٨٩";
            var persian = "۰۱۲۳۴۵۶۷۸۹";
            var sb = new System.Text.StringBuilder(s.Length);
            foreach (var c in s)
            {
                int idx = arabic.IndexOf(c);
                if (idx < 0) idx = persian.IndexOf(c);
                sb.Append(idx >= 0 ? (char)('0' + idx) : c);
            }
            return sb.ToString();
        }
    }
}
