using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Civil_Registration_System_Platform.Controllers
{

    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly ISuperAdminService _superAdminService;
        private readonly IAdminManagementService _adminManagementService;
        private readonly IPricingManagementService _pricingManagementService;
        private readonly IGovernorateServices _governorateServices;
        private readonly IOfficeServices _officeServices;

        public SuperAdminController(
            UserManager<UserAccount> userManager,
            ISuperAdminService superAdminService,
            IAdminManagementService adminManagementService,
            IPricingManagementService pricingManagementService,
            IGovernorateServices governorateServices,
            IOfficeServices officeServices)
        {
            _userManager = userManager;
            _superAdminService = superAdminService;
            _adminManagementService = adminManagementService;
            _pricingManagementService = pricingManagementService;
            _governorateServices = governorateServices;
            _officeServices = officeServices;
        }


        //  Index — GET /SuperAdmin 

        [HttpGet]
        public async Task<IActionResult> Index(
            ApplicationStatus? status = null,
            ServiceType? serviceType = null,
            int? officeId = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null)
        {
            var vm = await _superAdminService.GetDashboardAsync(
                status, serviceType, officeId, dateFrom, dateTo);
            return View(vm);
        }

      
        //  Admins — GET /SuperAdmin/Admins 

        [HttpGet]
        public async Task<IActionResult> Admins()
        {
            var admins = await _adminManagementService.GetAllAdminsAsync();
            return View(admins);
        }

 

        [HttpGet]
        public async Task<IActionResult> AdminDetails(string id)
        {
            var admin = await _adminManagementService.GetAdminAsync(id);

            if (admin == null)
            {
                TempData["Error"] = "لم يتم العثور على الأدمن";
                return RedirectToAction(nameof(Admins));
            }

            return View(admin);
        }

        //  CreateAdmin — GET /SuperAdmin/CreateAdmin 

        [HttpGet]
        public async Task<IActionResult> CreateAdmin()
        {
            ViewBag.Governorates = await _governorateServices.GetAllGovernoratesAsync();
            return View(new RegisterAdminOrEmployeeViewModel());
        }

        //  CreateAdmin — POST /SuperAdmin/CreateAdmin 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(RegisterAdminOrEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Governorates = await _governorateServices.GetAllGovernoratesAsync();
                return View(model);
            }

            try
            {
                model.OfficeId = null;
                model.ManagerOfficeId = null;
                var result = await _adminManagementService.CreateAdminAsync(model);

                if (result.Contains("Successfully", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["Success"] = "تم إنشاء حساب الأدمن بنجاح";
                    return RedirectToAction(nameof(Admins));
                }

                ModelState.AddModelError(string.Empty, result);
                ViewBag.Governorates = await _governorateServices.GetAllGovernoratesAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Governorates = await _governorateServices.GetAllGovernoratesAsync();
                return View(model);
            }
        }

        //  ToggleAdmin — POST /SuperAdmin/ToggleAdmin 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(string adminId)
        {
            try
            {
                var result = await _adminManagementService.ToggleAdminActiveAsync(adminId);
                TempData["Success"] = result;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Admins));
        }

     

        //  Pricing — GET /SuperAdmin/Pricing 

        [HttpGet]
        public async Task<IActionResult> Pricing()
        {
            var prices = await _pricingManagementService.GetAllPricesAsync();
            return View(prices);
        }

        //  CreatePrice — GET /SuperAdmin/CreatePrice 

        [HttpGet]
        public IActionResult CreatePrice() => View(new PricingFormVM());

        //  EditPrice — GET /SuperAdmin/EditPrice?serviceType=6&applicationType=1 

        [HttpGet]
        public async Task<IActionResult> EditPrice(ServiceType serviceType, ApplicationType applicationType)
        {
            var vm = await _pricingManagementService.GetPriceAsync(serviceType, applicationType);

            if (vm == null)
            {
                TempData["Error"] = "لم يتم العثور على السعر المطلوب";
                return RedirectToAction(nameof(Pricing));
            }

            return View(vm);
        }

        //  SavePrice — POST /SuperAdmin/SavePrice (Upsert) 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePrice(PricingFormVM model)
        {
            if (!ModelState.IsValid)
                return View("CreatePrice", model);

            try
            {
                var result = await _pricingManagementService.UpsertPriceAsync(model);
                TempData["Success"] = result;
                return RedirectToAction(nameof(Pricing));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("CreatePrice", model);
            }
        }

        //  DeletePrice — POST /SuperAdmin/DeletePrice 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePrice(ServiceType serviceType, ApplicationType applicationType)
        {
            try
            {
                var result = await _pricingManagementService.DeletePriceAsync(serviceType, applicationType);
                TempData["Success"] = result;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Pricing));
        }

     

        //  GetOffices — GET /SuperAdmin/GetOffices?governorateId=1 

        [HttpGet]
        public async Task<IActionResult> GetOffices(int governorateId)
        {
            var offices = await _officeServices.GetByGovernorateIdAsync(governorateId);
            return Json(offices);
        }
    }
}
