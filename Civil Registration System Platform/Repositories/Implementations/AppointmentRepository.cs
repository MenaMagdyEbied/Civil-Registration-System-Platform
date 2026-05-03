using Civil_Registration_System_Platform.Enums;
using Civil_Registration_System_Platform.Repositories.Implementations;
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
         .Where(a => a.Application.OfficeId == officeId
                  && !a.IsDeleted
                  && a.AppointmentDate.Date == date.Date)
         .Include(a => a.Application)
             .ThenInclude(app => app.UserAccount)
         .ToListAsync();

        public async Task<IEnumerable<Appointment>> GetUpcomingByUserIdAsync(string userId)
            => await _context.Appointments
                .Where(a => a.UserAccountId == userId
                         && !a.IsDeleted
                         && a.AppointmentDate >= DateTime.Today
                         && a.Status != (int)AppointmentStatus.Cancelled)
                .Include(a => a.Application)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
    }
}
