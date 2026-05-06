using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Civil_Registration_System_Platform.Controllers
{
    /// <summary>
    /// لوحة تحكم الأدمن/الموظف — إدارة طلبات المكتب + الموظفين + اعتماد المواطنين
    /// الأدمن يدير موظفي مكتبه — الموظف يراجع طلبات مكتبه فقط
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    public class AdminController : Controller
    {
        private readonly IAdminApplicationService _adminAppService;
        private readonly IEmployeeManagementService _employeeService;
        private readonly IAppointmentService _appointmentService;
        private readonly IAcccountManageServices _accountManageServices;
        private readonly IUserGlobalServices _globalServices;
        private readonly UserManager<UserAccount> _userManager;

        public AdminController(
            IAdminApplicationService adminAppService,
            IEmployeeManagementService employeeService,
            IAppointmentService appointmentService,
            IAcccountManageServices accountManageServices,
            IUserGlobalServices globalServices,
            UserManager<UserAccount> userManager)
        {
            _adminAppService = adminAppService;
            _employeeService = employeeService;
            _appointmentService = appointmentService;
            _accountManageServices = accountManageServices;
            _globalServices = globalServices;
            _userManager = userManager;
        }

        // ══════════════════════════════════════════════════════════════
        //  داشبورد المكتب
        // ══════════════════════════════════════════════════════════════

        // ───────── Index — GET /Admin ─────────

        [HttpGet]
        public async Task<IActionResult> Index(
            ApplicationStatus? status = null,
            ServiceType? serviceType = null)
        {
            var currentUser = await _globalServices.GetUser();
            int officeId = currentUser.ManageOfficeId ?? currentUser.OfficeId ?? 0;

            if (officeId == 0)
            {
                TempData["Error"] = "لم يتم تعيين مكتب لك. تواصل مع السوبر أدمن.";
                return RedirectToAction("Index", "Home");
            }

            var vm = await _adminAppService.GetOfficeDashboardAsync(officeId, status, serviceType);
            return View(vm);
        }

        // ══════════════════════════════════════════════════════════════
        //  إدارة الطلبات
        // ══════════════════════════════════════════════════════════════

        // ───────── Applications — GET /Admin/Applications ─────────

        [HttpGet]
        public async Task<IActionResult> Applications(
            ApplicationStatus? status = null,
            ServiceType? serviceType = null)
        {
            var currentUser = await _globalServices.GetUser();
            int officeId = currentUser.ManageOfficeId ?? currentUser.OfficeId ?? 0;

            var applications = await _adminAppService.GetOfficeApplicationsAsync(
                officeId, status, serviceType);
            return View(applications);
        }

        // ───────── Review — GET /Admin/Review/5 ─────────

        [HttpGet]
        public async Task<IActionResult> Review(int id)
        {
            var vm = await _adminAppService.GetReviewDetailsAsync(id);

            if (vm == null)
            {
                TempData["Error"] = "لم يتم العثور على الطلب";
                return RedirectToAction(nameof(Applications));
            }

            return View(vm);
        }

        // ───────── Approve — POST /Admin/Approve ─────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int applicationId, string? notes)
        {
            try
            {
                var adminId = _userManager.GetUserId(User);
                await _adminAppService.ApproveApplicationAsync(applicationId, adminId, notes);
                TempData["Success"] = "تم قبول الطلب بنجاح";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Review), new { id = applicationId });
        }

        // ───────── Reject — POST /Admin/Reject ─────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int applicationId, string reason)
        {
            try
            {
                var adminId = _userManager.GetUserId(User);
                await _adminAppService.RejectApplicationAsync(applicationId, adminId, reason);
                TempData["Success"] = "تم رفض الطلب";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Review), new { id = applicationId });
        }

        // ───────── RequestInfo — POST /Admin/RequestInfo ─────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestInfo(int applicationId, string details)
        {
            try
            {
                var adminId = _userManager.GetUserId(User);
                await _adminAppService.RequestAdditionalInfoAsync(applicationId, adminId, details);
                TempData["Success"] = "تم طلب معلومات إضافية من المتقدم";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Review), new { id = applicationId });
        }

        // ───────── Issue — POST /Admin/Issue ─────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Issue(int applicationId)
        {
            try
            {
                var adminId = _userManager.GetUserId(User);
                await _adminAppService.IssueApplicationAsync(applicationId, adminId);
                TempData["Success"] = "تم إصدار المستند بنجاح";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Review), new { id = applicationId });
        }

        // ───────── ChangeStatus — POST /Admin/ChangeStatus (عام) ─────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(
            int applicationId, ApplicationStatus newStatus, string? note)
        {
            try
            {
                var adminId = _userManager.GetUserId(User);
                await _adminAppService.ChangeStatusAsync(applicationId, newStatus, adminId, note);
                TempData["Success"] = "تم تحديث حالة الطلب";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Review), new { id = applicationId });
        }

        // ══════════════════════════════════════════════════════════════
        //  المواعيد
        // ══════════════════════════════════════════════════════════════

        // ───────── Schedule — GET /Admin/Schedule?date=2025-06-01 ─────────

        [HttpGet]
        public async Task<IActionResult> Schedule(DateTime? date = null)
        {
            var currentUser = await _globalServices.GetUser();
            int officeId = currentUser.ManageOfficeId ?? currentUser.OfficeId ?? 0;

            var targetDate = date ?? DateTime.Today;
            var appointments = await _appointmentService.GetOfficeScheduleAsync(officeId, targetDate);

            ViewBag.SelectedDate = targetDate;
            return View(appointments);
        }

        // ───────── ScheduleAppointment — POST /Admin/ScheduleAppointment ─────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScheduleAppointment(ScheduleAppointmentVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "بيانات الموعد غير صحيحة";
                return RedirectToAction(nameof(Review), new { id = model.ApplicationId });
            }

            try
            {
                var adminId = _userManager.GetUserId(User);
                await _appointmentService.ScheduleAsync(
                    model.ApplicationId,
                    model.AppointmentDate,
                    model.AppointmentTime,
                    adminId,
                    model.TargetStatus);   // Medical / Theory / Practical للرخصة
                TempData["Success"] = "تم تحديد الموعد بنجاح";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Review), new { id = model.ApplicationId });
        }

        // ───────── CompleteAppointment — POST /Admin/CompleteAppointment ─────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteAppointment(int appointmentId, int applicationId)
        {
            try
            {
                var adminId = _userManager.GetUserId(User);
                await _appointmentService.MarkAsCompletedAsync(appointmentId, adminId);
                TempData["Success"] = "تم تأكيد حضور الموعد";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Review), new { id = applicationId });
        }

        // ───────── NoShowAppointment — POST /Admin/NoShowAppointment ─────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NoShowAppointment(int appointmentId, int applicationId)
        {
            try
            {
                var adminId = _userManager.GetUserId(User);
                await _appointmentService.MarkAsNoShowAsync(appointmentId, adminId);
                TempData["Success"] = "تم تسجيل عدم حضور";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Review), new { id = applicationId });
        }

        // ───────── CancelAppointment — POST /Admin/CancelAppointment ─────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment(
            int appointmentId, int applicationId, string? reason)
        {
            try
            {
                var adminId = _userManager.GetUserId(User);
                await _appointmentService.CancelAsync(appointmentId, adminId, reason);
                TempData["Success"] = "تم إلغاء الموعد";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Review), new { id = applicationId });
        }

        // ══════════════════════════════════════════════════════════════
        //  إدارة الموظفين — الأدمن فقط
        // ══════════════════════════════════════════════════════════════

        // ───────── Employees — GET /Admin/Employees ─────────

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Employees()
        {
            var currentUser = await _globalServices.GetUser();
            int officeId = currentUser.ManageOfficeId ?? currentUser.OfficeId ?? 0;

            var employees = await _employeeService.GetEmployeesByOfficeAsync(officeId);
            return View(employees);
        }

        // ───────── CreateEmployee — GET /Admin/CreateEmployee ─────────

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateEmployee() => View(new RegisterAdminOrEmployeeViewModel());

        // ───────── CreateEmployee — POST /Admin/CreateEmployee ─────────

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(RegisterAdminOrEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _employeeService.CreateEmployeeAsync(model);

                if (result.Contains("Successfully", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["Success"] = "تم إنشاء حساب الموظف بنجاح";
                    return RedirectToAction(nameof(Employees));
                }

                ModelState.AddModelError(string.Empty, result);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // ───────── ToggleEmployee — POST /Admin/ToggleEmployee ─────────

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleEmployee(string employeeId)
        {
            try
            {
                var currentUser = await _globalServices.GetUser();
                int officeId = currentUser.ManageOfficeId ?? currentUser.OfficeId ?? 0;

                var result = await _employeeService.ToggleEmployeeActiveAsync(employeeId, officeId);
                TempData["Success"] = result;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Employees));
        }

        // ══════════════════════════════════════════════════════════════
        //  اعتماد المواطنين — الأدمن/الموظف
        // ══════════════════════════════════════════════════════════════

        // ───────── UnconfirmedUsers — GET /Admin/UnconfirmedUsers ─────────
        // Admin-only: الموظفين مش مسموحلهم باعتماد المواطنين
        // (الـ underlying CheckIfIsAdmin بترفض حد غير الأدمن أصلاً)

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnconfirmedUsers()
        {
            var users = await _accountManageServices.GetUnConfirmedUserAsync();
            return View(users);
        }

        // ───────── ConfirmUser — POST /Admin/ConfirmUser ─────────
        // الأدمن بيقارن البيانات المُسجَّلة مع صورة البطاقة ثم يعتمد

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmUser(string userId)
        {
            try
            {
                var result = await _accountManageServices.ConfrimUser(userId);
                TempData["Success"] = result;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(UnconfirmedUsers));
        }

        // ───────── RejectUser — POST /Admin/RejectUser ─────────

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectUser(string userId, string? messageReject)
        {
            try
            {
                var result = await _accountManageServices.RejectUser(userId, messageReject);
                TempData["Success"] = result;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(UnconfirmedUsers));
        }

        // ───────── OfficeUsers — GET /Admin/OfficeUsers ─────────

        [HttpGet]
        public async Task<IActionResult> OfficeUsers()
        {
            var currentUser = await _globalServices.GetUser();
            int officeId = currentUser.ManageOfficeId ?? currentUser.OfficeId ?? 0;

            var users = await _accountManageServices.GetAllUsersByOfficeAsync(officeId);
            return View(users);
        }
    }
}
