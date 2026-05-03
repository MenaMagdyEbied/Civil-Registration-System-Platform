namespace Civil_Registration_System_Platform.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task SoftDeleteAsync(int id);     
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync();
    }
}
