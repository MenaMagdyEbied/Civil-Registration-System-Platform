namespace Civil_Registration_System_Platform.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadDocumentAsync(IFormFile file, int applicationId);
        Task DeleteDocumentAsync(string filePath);
        bool IsValidFileType(IFormFile file);
        bool IsValidFileSize(IFormFile file, int maxSizeMB = 5);
    }
}
