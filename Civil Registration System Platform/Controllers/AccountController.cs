using Civil_Registration_System_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Civil_Registration_System_Platform.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly IAccountServices _accountServices;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly UserManager<UserAccount> _userManager;

        public AccountController(
            IAccountServices accountServices,
            SignInManager<UserAccount> signInManager,
            UserManager<UserAccount> userManager)
        {
            _accountServices = accountServices;
            _signInManager   = signInManager;
            _userManager     = userManager;
        }


        [HttpGet]
        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountServices.RegisterUserAsync(model);

            if (result.Contains("Successfully", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Success"] = "تم إنشاء الحساب بنجاح. يمكنك تسجيل الدخول الآن";
                return RedirectToAction(nameof(Login));
            }

            ModelState.AddModelError(string.Empty, result);
            return View(model);
        }


        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountServices.LoginUserAsync(model);

            if (!result.Equals("Login Successful", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "البريد/اسم المستخدم أو كلمة المرور غير صحيحة");
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName)
                    ?? await _userManager.FindByEmailAsync(model.UserName)
                    ?? await _userManager.Users.FirstOrDefaultAsync(u => u.NationalID == model.UserName);

            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                    return RedirectToAction("Index", "SuperAdmin");

                if (await _userManager.IsInRoleAsync(user, "AccountReviewer"))
                    return RedirectToAction("UnconfirmedUsers", "Admin");

                if (await _userManager.IsInRoleAsync(user, "Admin")
                 || await _userManager.IsInRoleAsync(user, "Employee"))
                    return RedirectToAction("Index", "Admin");

                if (await _userManager.IsInRoleAsync(user, "User"))
                {
                    if (!user.IsConfirmed)
                        TempData["Info"] = "حسابك بانتظار اعتماد الأدمن — لن تتمكن من تقديم طلبات حتى يتم الاعتماد";

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyAccount()
        {
            var vm = await _accountServices.GetMyAccount();
            return View(vm);
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditAccount() => View(new UserAccountEdit());

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(UserAccountEdit model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _accountServices.EditMyAccount(model);
                TempData["Success"] = result;
                return RedirectToAction(nameof(MyAccount));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }


        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}
