namespace Civil_Registration_System_Platform.Helpers
{
    /// <summary>
    /// ⚠️  DEPRECATED — runtime seeding moved to AppDbContext.OnModelCreating via HasData
    /// (see seed ServicesTypeHelper region).
    ///
    /// لتطبيق البيانات على قاعدة البيانات:
    ///     dotnet ef migrations add SeedServicesTypeHelperData
    ///     dotnet ef database update
    ///
    /// تركنا الكلاس كـ no-op لو في كود قديم بيندهه.
    /// </summary>
    public static class ServicesTypeHelperSeeder
    {
        public static Task SeedAsync(AppDbContext context) => Task.CompletedTask;
    }
}
