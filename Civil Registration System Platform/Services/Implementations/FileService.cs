using Civil_Registration_System_Platform.Services.Interfaces;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        private readonly string[] _allowedTypes =
        {
        "application/pdf",
        "image/jpeg",
        "image/png"
    };

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadDocumentAsync(IFormFile file, int applicationId)
        {
            if (!IsValidFileType(file))
                throw new InvalidOperationException("نوع الملف غير مسموح. يقبل PDF والصور فقط");

            if (!IsValidFileSize(file))
                throw new InvalidOperationException("حجم الملف يتجاوز الحد المسموح (5MB)");

            // الباث: wwwroot/uploads/2025/05/
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.Month.ToString("D2");
            var folderPath = Path.Combine(
                _env.WebRootPath, "uploads", year, month);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var extension = Path.GetExtension(file.FileName);
            var uniqueName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(folderPath, uniqueName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Path.Combine("uploads", year, month, uniqueName);
        }

        public Task DeleteDocumentAsync(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
            return Task.CompletedTask;
        }

        public bool IsValidFileType(IFormFile file)
            => _allowedTypes.Contains(file.ContentType.ToLower());

        public bool IsValidFileSize(IFormFile file, int maxSizeMB = 5)
            => file.Length <= maxSizeMB * 1024 * 1024;
    }
}
