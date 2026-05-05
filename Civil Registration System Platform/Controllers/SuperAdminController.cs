using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Civil_Registration_System_Platform.Controllers
{
    [Authorize(Roles = "superadmin")]
    public class SuperAdminController : Controller
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly IAccountServices _accountServices;

        public SuperAdminController(
            UserManager<UserAccount> userManager,
            IAccountServices accountServices)
        {
            _userManager = userManager;
            _accountServices = accountServices;
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


        [HttpGet]
        public IActionResult AddAdmin()
        {
            return View(new RegisterAdminOrEmployeeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(RegisterAdminOrEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountServices.RegisterAdminAsync(model);

            if (IsSuccess(result))
            {
                TempData["SuccessMessage"] = "تم إضافة الأدمن بنجاح.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "معرف الأدمن مطلوب.";
                return RedirectToAction(nameof(Index));
            }

            var admin = await _userManager.FindByIdAsync(id);

            if (admin == null)
            {
                TempData["ErrorMessage"] = "الأدمن غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            return View(admin);
        }


        [HttpGet]
        public async Task<IActionResult> EditAdmin(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "معرف الأدمن مطلوب.";
                return RedirectToAction(nameof(Index));
            }

            var admin = await _userManager.FindByIdAsync(id);

            if (admin == null)
            {
                TempData["ErrorMessage"] = "الأدمن غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            var model = new EditAdminViewModel
            {
                Id = admin.Id,
                FullName = admin.FullName,
                Email = admin.Email ?? string.Empty,
                EGPhoneNumber = admin.EGPhoneNumber,
                NationalID = admin.NationalID,
                OfficeId = admin.OfficeId,
                ManageOfficeId = admin.ManageOfficeId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmin(EditAdminViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = await _userManager.FindByIdAsync(model.Id);

            if (admin == null)
            {
                TempData["ErrorMessage"] = "الأدمن غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            admin.FullName = model.FullName;
            admin.EGPhoneNumber = model.EGPhoneNumber;
            admin.NationalID = model.NationalID;
            admin.OfficeId = model.OfficeId;
            admin.ManageOfficeId = model.ManageOfficeId;

            if (admin.Email != model.Email)
                await _userManager.SetEmailAsync(admin, model.Email);

            var result = await _userManager.UpdateAsync(admin);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "تم تعديل بيانات الأدمن بنجاح.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAdmin(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "معرف الأدمن مطلوب.";
                return RedirectToAction(nameof(Index));
            }

            var admin = await _userManager.FindByIdAsync(id);

            if (admin == null)
            {
                TempData["ErrorMessage"] = "الأدمن غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            return View(admin);
        }

        [HttpPost]
        [ActionName("DeleteAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdminConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "معرف الأدمن مطلوب.";
                return RedirectToAction(nameof(Index));
            }

            var admin = await _userManager.FindByIdAsync(id);

            if (admin == null)
            {
                TempData["ErrorMessage"] = "الأدمن غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(admin);

            TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
                result.Succeeded ? "تم حذف الأدمن بنجاح." : "حدث خطأ أثناء الحذف.";

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "معرف الأدمن مطلوب.";
                return RedirectToAction(nameof(Index));
            }

            var admin = await _userManager.FindByIdAsync(id);

            if (admin == null)
            {
                TempData["ErrorMessage"] = "الأدمن غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            return View(new ResetAdminPasswordViewModel { Id = id, FullName = admin.FullName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetAdminPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = await _userManager.FindByIdAsync(model.Id);

            if (admin == null)
            {
                TempData["ErrorMessage"] = "الأدمن غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(admin);
            var result = await _userManager.ResetPasswordAsync(admin, token, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "تم إعادة تعيين كلمة السر بنجاح.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }


        private static bool IsSuccess(string result) =>
            result == "Success" ||
            result.Contains("Successfully") ||
            result.Contains("بنجاح") ||
            result.Contains("تم");


    }
}