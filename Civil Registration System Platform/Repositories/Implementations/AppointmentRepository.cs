using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Implementations
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context) { }

        public async Task<Appointment?> GetByApplicationIdAsync(int applicationId)
            => await _context.Appointments
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId && !a.IsDeleted);
        public async Task<IEnumerable<Appointment>> GetByOfficeAndDateAsync(int officeId, DateTime date)
            => await _context.Appointments
                .Include(a => a.Application)
                    .ThenInclude(app => app.Office)
                .Include(a => a.Application)
                    .ThenInclude(app => app.UserAccount)
                .Where(a => !a.IsDeleted
                         && a.Application.OfficeId == officeId
                         && a.AppointmentDate.Date == date.Date)
                .ToListAsync();

        public async Task<IEnumerable<Appointment>> GetUpcomingByUserIdAsync(string userId)
            => await _context.Appointments
                .Include(a => a.Application)
                    .ThenInclude(app => app.Office)
                .Where(a => a.UserAccountId == userId
                         && !a.IsDeleted
                         && a.AppointmentDate >= DateTime.Today
                         && a.Status != (int)AppointmentStatus.Cancelled)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
    }
}
