using Civil_Registration_System_Platform.Repositories.Implementations;
using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Implementations
{
    public class ApplicationDocumentRepository
        : GenericRepository<ApplicationDocuments>, IApplicationDocumentRepository
    {
        public ApplicationDocumentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<ApplicationDocuments>> GetByApplicationIdAsync(int applicationId)
            => await _context.ApplicationDocuments
                .Where(d => d.ApplicationId == applicationId && !d.IsDeleted)
                .ToListAsync();

        public async Task SoftDeleteByApplicationIdAsync(int applicationId)
        {
            var docs = await _context.ApplicationDocuments
                .Where(d => d.ApplicationId == applicationId && !d.IsDeleted)
                .ToListAsync();

            foreach (var doc in docs)
                doc.IsDeleted = true;
        }
    }
}
