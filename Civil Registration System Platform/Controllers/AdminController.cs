using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Civil_Registration_System_Platform.Controllers
{
    [Authorize(Roles = "Admin,Employee,AccountReviewer")]
    public class AdminController : Controller
    {
        private readonly IAdminApplicationService _adminAppService;
        private readonly IEmployeeManagementService _employeeService;
        private readonly IAppointmentService _appointmentService;
        private readonly IAcccountManageServices _accountManageServices;
        private readonly IUserGlobalServices _globalServices;
        private readonly IOfficeServices _officeServices;
        private readonly UserManager<UserAccount> _userManager;

        public AdminController(
            IAdminApplicationService adminAppService,
            IEmployeeManagementService employeeService,
            IAppointmentService appointmentService,
            IAcccountManageServices accountManageServices,
            IUserGlobalServices globalServices,
            IOfficeServices officeServices,
            UserManager<UserAccount> userManager)
        {
            _adminAppService = adminAppService;
            _employeeService = employeeService;
            _appointmentService = appointmentService;
            _accountManageServices = accountManageServices;
            _globalServices = globalServices;
            _officeServices = officeServices;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            ApplicationStatus? status = null,
            ServiceType? serviceType = null)
        {
            if (User.IsInRole("AccountReviewer"))
                return RedirectToAction(nameof(UnconfirmedUsers));

            var officeIds = await GetAccessibleOfficeIdsAsync();
            if (!officeIds.Any())
            {
                TempData["Error"] = "لا يوجد مكتب مرتبط بهذا الحساب.";
                return RedirectToAction("Index", "Home");
            }

            var vm = await _adminAppService.GetOfficesDashboardAsync(officeIds, status, serviceType);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Applications(
            ApplicationStatus? status = null,
            ServiceType? serviceType = null)
        {
            var applications = await _adminAppService.GetOfficesApplicationsAsync(
                await GetAccessibleOfficeIdsAsync(), status, serviceType);
            return View(applications);
        }

        [HttpGet]
        public async Task<IActionResult> Review(int id)
        {
            var vm = await _adminAppService.GetReviewDetailsAsync(id);
            if (vm == null || !await CanAccessOfficeAsync(vm.OfficeId))
            {
                TempData["Error"] = "لم يتم العثور على الطلب داخل نطاق المكتب المخصص لك.";
                return RedirectToAction(nameof(Applications));
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int applicationId, string? notes)
            => await RunApplicationActionAsync(
                applicationId,
                () => _adminAppService.ApproveApplicationAsync(applicationId, _userManager.GetUserId(User)!, notes),
                "تمت الموافقة على الطلب بنجاح");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int applicationId, string reason)
            => await RunApplicationActionAsync(
                applicationId,
                () => _adminAppService.RejectApplicationAsync(applicationId, _userManager.GetUserId(User)!, reason),
                "تم رفض الطلب");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestInfo(int applicationId, string details)
            => await RunApplicationActionAsync(
                applicationId,
                () => _adminAppService.RequestAdditionalInfoAsync(applicationId, _userManager.GetUserId(User)!, details),
                "تم طلب معلومات إضافية");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Issue(int applicationId)
            => await RunApplicationActionAsync(
                applicationId,
                () => _adminAppService.IssueApplicationAsync(applicationId, _userManager.GetUserId(User)!),
                "تم إصدار المستند بنجاح");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(
            int applicationId, ApplicationStatus newStatus, string? note)
            => await RunApplicationActionAsync(
                applicationId,
                () => _adminAppService.ChangeStatusAsync(applicationId, newStatus, _userManager.GetUserId(User)!, note),
                "تم تحديث حالة الطلب");

        [HttpGet]
        public async Task<IActionResult> Schedule(DateTime? date = null)
        {
            var targetDate = date ?? DateTime.Today;
            var appointments = new List<AppointmentSummaryVM>();

            foreach (var officeId in await GetAccessibleOfficeIdsAsync())
                appointments.AddRange(await _appointmentService.GetOfficeScheduleAsync(officeId, targetDate));

            ViewBag.SelectedDate = targetDate;
            return View(appointments.OrderBy(a => a.AppointmentDate));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScheduleAppointment(ScheduleAppointmentVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "بيانات الموعد غير صحيحة.";
                return RedirectToAction(nameof(Review), new { id = model.ApplicationId });
            }

            return await RunApplicationActionAsync(
                model.ApplicationId,
                () => _appointmentService.ScheduleAsync(
                    model.ApplicationId,
                    model.AppointmentDate,
                    model.AppointmentTime,
                    _userManager.GetUserId(User)!,
                    model.TargetStatus),
                "تم تحديد الموعد بنجاح");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteAppointment(int appointmentId, int applicationId)
            => await RunApplicationActionAsync(
                applicationId,
                () => _appointmentService.MarkAsCompletedAsync(appointmentId, _userManager.GetUserId(User)!),
                "تم إكمال الموعد");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NoShowAppointment(int appointmentId, int applicationId)
            => await RunApplicationActionAsync(
                applicationId,
                () => _appointmentService.MarkAsNoShowAsync(appointmentId, _userManager.GetUserId(User)!),
                "تم تسجيل عدم الحضور");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment(
            int appointmentId, int applicationId, string? reason)
            => await RunApplicationActionAsync(
                applicationId,
                () => _appointmentService.CancelAsync(appointmentId, _userManager.GetUserId(User)!, reason),
                "تم إلغاء الموعد");

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Employees()
        {
            var employees = await _employeeService.GetEmployeesByOfficesAsync(await GetAccessibleOfficeIdsAsync());
            return View(employees);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmployee()
        {
            await FillEmployeeOfficeListAsync();
            return View(new RegisterAdminOrEmployeeViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(RegisterAdminOrEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await FillEmployeeOfficeListAsync();
                return View(model);
            }

            try
            {
                var currentUser = await _globalServices.GetUser();
                var allowedOfficeIds = await GetAccessibleOfficeIdsAsync();

                if (!model.OfficeId.HasValue || !allowedOfficeIds.Contains(model.OfficeId.Value))
                    throw new Exception("هذا المكتب خارج نطاق محافظتك.");

                model.GovernorateId = currentUser.GovernorateId ?? model.GovernorateId;
                model.ManagerOfficeId = null;

                var result = await _employeeService.CreateEmployeeAsync(model);
                if (result.Contains("Successfully", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["Success"] = "تم إنشاء حساب الموظف بنجاح.";
                    return RedirectToAction(nameof(Employees));
                }

                ModelState.AddModelError(string.Empty, result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            await FillEmployeeOfficeListAsync();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleEmployee(string employeeId)
        {
            try
            {
                var result = await _employeeService.ToggleEmployeeActiveAsync(
                    employeeId, await GetAccessibleOfficeIdsAsync());
                TempData["Success"] = result;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Employees));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,AccountReviewer")]
        public async Task<IActionResult> UnconfirmedUsers()
        {
            var users = await _accountManageServices.GetUnConfirmedUserAsync();
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,AccountReviewer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmUser(string userId)
        {
            try
            {
                TempData["Success"] = await _accountManageServices.ConfrimUser(userId);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(UnconfirmedUsers));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,AccountReviewer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectUser(string userId, string? messageReject)
        {
            try
            {
                TempData["Success"] = await _accountManageServices.RejectUser(userId, messageReject);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(UnconfirmedUsers));
        }

        [HttpGet]
        public async Task<IActionResult> OfficeUsers()
        {
            var officeIds = await GetAccessibleOfficeIdsAsync();
            var allUsers = new List<OfficeUserListItemVM>();

            foreach (var officeId in officeIds)
            {
                var users = await _accountManageServices.GetAllUsersByOfficeAsync(officeId);
                if (users != null) allUsers.AddRange(users);
            }

            return View(allUsers.OrderBy(u => u.FullName));
        }

        private async Task<IActionResult> RunApplicationActionAsync(
            int applicationId,
            Func<Task> action,
            string successMessage)
        {
            try
            {
                if (User.IsInRole("Admin") && !User.IsInRole("Employee"))
                {
                    throw new Exception("للأدمن صلاحية العرض فقط. تنفيذ الإجراءات (قبول/رفض/إصدار) من مهام الموظف المختص.");
                }

                var vm = await _adminAppService.GetReviewDetailsAsync(applicationId);
                if (vm == null || !await CanAccessOfficeAsync(vm.OfficeId))
                    throw new Exception("لا يمكنك مراجعة طلبات خارج نطاق المكتب المخصص لك.");

                await action();
                TempData["Success"] = successMessage;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Review), new { id = applicationId });
        }

        private async Task<List<int>> GetAccessibleOfficeIdsAsync()
        {
            var currentUser = await _globalServices.GetUser();

            if (await _userManager.IsInRoleAsync(currentUser, "Admin") && currentUser.GovernorateId.HasValue)
            {
                return (await _officeServices.GetByGovernorateIdAsync(currentUser.GovernorateId.Value))
                    .Select(o => o.OfficeId)
                    .ToList();
            }

            var officeId = currentUser.OfficeId ?? currentUser.ManageOfficeId;
            return officeId.HasValue ? new List<int> { officeId.Value } : new List<int>();
        }

        private async Task<bool> CanAccessOfficeAsync(int officeId)
            => (await GetAccessibleOfficeIdsAsync()).Contains(officeId);

        private async Task FillEmployeeOfficeListAsync()
        {
            var currentUser = await _globalServices.GetUser();
            ViewBag.GovernorateId = currentUser.GovernorateId;
            ViewBag.Offices = currentUser.GovernorateId.HasValue
                ? (await _officeServices.GetByGovernorateIdAsync(currentUser.GovernorateId.Value)).ToList()
                : new List<OfficeGovernrate>();
        }
    }
}
