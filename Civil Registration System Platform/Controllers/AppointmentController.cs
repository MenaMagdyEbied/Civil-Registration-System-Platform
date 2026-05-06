using Civil_Registration_System_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Civil_Registration_System_Platform.Controllers
{
        [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserGlobalServices _userGlobalServices;

        public AppointmentController(
            IAppointmentService appointmentService,
            IUserGlobalServices userGlobalServices)
        {
            _appointmentService = appointmentService;
            _userGlobalServices = userGlobalServices;
        }

    
        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> OfficeSchedule(DateTime? date = null)
        {
            var user = await _userGlobalServices.GetUser();

            if (user.ManageOfficeId == null && user.OfficeId == null)
            {
                TempData["ErrorMessage"] = "لا يوجد مكتب مرتبط بحسابك. تواصل مع المسؤول.";
                return RedirectToAction("Index", "Home");
            }

            int officeId = user.ManageOfficeId ?? user.OfficeId!.Value;
            var targetDate = date ?? DateTime.Today;
            var schedule = await _appointmentService.GetOfficeScheduleAsync(officeId, targetDate);

            ViewBag.SelectedDate = targetDate;
            return View(schedule); 
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Schedule(int applicationId, DateTime date, TimeSpan time)
        {
            if (applicationId <= 0)
            {
                TempData["ErrorMessage"] = "معرف الطلب غير صحيح.";
                return RedirectToAction(nameof(OfficeSchedule));
            }

            if (date.Date < DateTime.Today)
            {
                TempData["ErrorMessage"] = "لا يمكن تحديد موعد في تاريخ ماضٍ.";
                return RedirectToAction(nameof(OfficeSchedule));
            }

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(adminId)) return Unauthorized();

            // BUG-04: ScheduleAsync بتجيب userId المواطن من الـ Application داخلياً
            await _appointmentService.ScheduleAsync(applicationId, date, time, adminId);
            TempData["SuccessMessage"] = "تم تحديد الموعد بنجاح.";
            return RedirectToAction(nameof(OfficeSchedule));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCompleted(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "معرف الموعد غير صحيح.";
                return RedirectToAction(nameof(OfficeSchedule));
            }

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(adminId)) return Unauthorized();

            await _appointmentService.MarkAsCompletedAsync(id, adminId);
            TempData["SuccessMessage"] = "تم تسجيل الموعد كمكتمل.";
            return RedirectToAction(nameof(OfficeSchedule));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkNoShow(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "معرف الموعد غير صحيح.";
                return RedirectToAction(nameof(OfficeSchedule));
            }

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(adminId)) return Unauthorized();

            await _appointmentService.MarkAsNoShowAsync(id, adminId);
            TempData["SuccessMessage"] = "تم تسجيل الغياب.";
            return RedirectToAction(nameof(OfficeSchedule));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id, string? reason)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "معرف الموعد غير صحيح.";
                return RedirectToAction(nameof(OfficeSchedule));
            }

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(adminId)) return Unauthorized();

            await _appointmentService.CancelAsync(id, adminId, reason);
            TempData["SuccessMessage"] = "تم إلغاء الموعد.";
            return RedirectToAction(nameof(OfficeSchedule));
        }
    }
}
