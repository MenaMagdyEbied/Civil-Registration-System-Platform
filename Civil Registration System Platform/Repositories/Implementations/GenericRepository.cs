using Civil_Registration_System_Platform.Repositories.Interfaces;

namespace Civil_Registration_System_Platform.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        { 
           return await _dbSet.ToListAsync(); 
        }


        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
        public async Task SoftDeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return;
            var prop = entity.GetType().GetProperty("IsDeleted");
            if (prop != null) prop.SetValue(entity, true);
        }

        public async Task<bool> ExistsAsync(int id)
        {
           return await _dbSet.FindAsync(id) != null;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
