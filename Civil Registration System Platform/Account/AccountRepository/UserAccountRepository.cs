using Civil_Registration_System_Platform.GlobalServices.GlobalInterface;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Civil_Registration_System_Platform.Account.AccountRepository
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserAccount> _userManager;
        private readonly IUserGlobalServices _userGlobalServices;

        public UserAccountRepository(
            AppDbContext context,
            IUserGlobalServices userGlobalServices,
            UserManager<UserAccount> userManager)
        {
            _context = context;
            _userGlobalServices = userGlobalServices;
            _userManager = userManager;
        }

        //  existing methods 

        public async Task<List<UserAccount>> GetUnConfirmedUserAsync()
        {
            UserAccount userLogin = await _userGlobalServices.GetUser();
            await _userGlobalServices.CheckIfCanReviewAccounts(); // Admin أو AccountReviewer
            var isReviewer = await _userGlobalServices.IsAccountReviewer();

            var query = _context.UserAccounts
                .Where(u => !u.IsConfirmed && !u.IsRejected);

            if (!isReviewer)
            {
                var officeIds = await _context.Offices
                    .Where(o => userLogin.GovernorateId.HasValue
                             && o.GovernorateId == userLogin.GovernorateId.Value)
                    .Select(o => o.OfficeId)
                    .ToListAsync();

                query = query.Where(u => u.OfficeId.HasValue && officeIds.Contains(u.OfficeId.Value));
            }

            return await query
                .Include(u => u.Governorate)
                .Include(u => u.Office)
                .ToListAsync();
        }

        public async Task<UserAccount> GetById(string userId)
        {
            UserAccount userLogin = await _userGlobalServices.GetUser();
            await _userGlobalServices.CheckIfCanReviewAccounts(); // Admin أو AccountReviewer
            var isReviewer = await _userGlobalServices.IsAccountReviewer();

            var query = _context.UserAccounts.AsQueryable();
            if (!isReviewer)
            {
                var officeIds = await _context.Offices
                    .Where(o => userLogin.GovernorateId.HasValue
                             && o.GovernorateId == userLogin.GovernorateId.Value)
                    .Select(o => o.OfficeId)
                    .ToListAsync();

                query = query.Where(u => u.OfficeId.HasValue && officeIds.Contains(u.OfficeId.Value));
            }

            UserAccount? userAccount = await query
                .SingleOrDefaultAsync(u => u.Id == userId);

            return userAccount;
        }

        public async Task<UserAccount> SaveUser(UserAccount userAccount)
        {
            var result = await _userManager.UpdateAsync(userAccount);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return userAccount;
        }

        public async Task<UserAccount> GetMyAccount()
        {
            UserAccount userLogin = await _userGlobalServices.GetUser();
            return userLogin;
        }


        public async Task<List<UserAccount>> GetAdminsAsync()
        {
            var inRole = await _userManager.GetUsersInRoleAsync("Admin");
            var ids = inRole.Select(u => u.Id).ToList();

            return await _context.UserAccounts
                .Where(u => ids.Contains(u.Id))
                .Include(u => u.Governorate)
                .Include(u => u.Office)
                .Include(u => u.ManageOffice)
                .ToListAsync();
        }

        public async Task<UserAccount?> GetAdminByIdAsync(string adminId)
        {
            var user = await _context.UserAccounts
                .Include(u => u.Governorate)
                .Include(u => u.Office)
                .Include(u => u.ManageOffice)
                .FirstOrDefaultAsync(u => u.Id == adminId);

            if (user == null) return null;

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            return isAdmin ? user : null;
        }

        public async Task<List<UserAccount>> GetEmployeesByOfficeAsync(int officeId)
            => await GetEmployeesByOfficesAsync(new[] { officeId });

        public async Task<List<UserAccount>> GetEmployeesByOfficesAsync(IEnumerable<int> officeIds)
        {
            var inRole = await _userManager.GetUsersInRoleAsync("Employee");
            var ids = inRole.Select(u => u.Id).ToList();
            var offices = officeIds.Distinct().ToList();

            return await _context.UserAccounts
                .Where(u => ids.Contains(u.Id) && u.OfficeId.HasValue && offices.Contains(u.OfficeId.Value))
                .Include(u => u.Governorate)
                .Include(u => u.Office)
                .ToListAsync();
        }

        public async Task<UserAccount?> GetEmployeeByIdAsync(string employeeId, int officeId)
            => await GetEmployeeByIdAsync(employeeId, new[] { officeId });

        public async Task<UserAccount?> GetEmployeeByIdAsync(string employeeId, IEnumerable<int> officeIds)
        {
            var offices = officeIds.Distinct().ToList();
            var user = await _context.UserAccounts
                .Include(u => u.Governorate)
                .Include(u => u.Office)
                .FirstOrDefaultAsync(u => u.Id == employeeId
                                       && u.OfficeId.HasValue
                                       && offices.Contains(u.OfficeId.Value));

            if (user == null) return null;

            var isEmployee = await _userManager.IsInRoleAsync(user, "Employee");
            return isEmployee ? user : null;
        }

        public async Task<List<UserAccount>> GetConfirmedUsersByOfficeAsync(int officeId)
        {
            var inRole = await _userManager.GetUsersInRoleAsync("User");
            var ids = inRole.Select(u => u.Id).ToList();

            return await _context.UserAccounts
                .Where(u => ids.Contains(u.Id)
                         && u.OfficeId == officeId
                         && u.IsConfirmed
                         && !u.IsRejected)
                .Include(u => u.Governorate)
                .Include(u => u.Office)
                .ToListAsync();
        }

        public async Task<List<UserAccount>> GetRejectedUsersByOfficeAsync(int officeId)
        {
            var inRole = await _userManager.GetUsersInRoleAsync("User");
            var ids = inRole.Select(u => u.Id).ToList();

            return await _context.UserAccounts
                .Where(u => ids.Contains(u.Id)
                         && u.OfficeId == officeId
                         && u.IsRejected)
                .Include(u => u.Governorate)
                .Include(u => u.Office)
                .ToListAsync();
        }

        public async Task<List<UserAccount>> GetAllUsersByOfficeAsync(int officeId)
        {
            var inRole = await _userManager.GetUsersInRoleAsync("User");
            var ids = inRole.Select(u => u.Id).ToList();

            return await _context.UserAccounts
                .Where(u => ids.Contains(u.Id) && u.OfficeId == officeId)
                .Include(u => u.Governorate)
                .Include(u => u.Office)
                .ToListAsync();
        }
    }
}
