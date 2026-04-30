namespace Civil_Registration_System_Platform.Models
{
    public class ApplicationDocuments
    {
        [Key]
        public int ApplicationDocumentsId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string DocumentPath { get; set; } // Path where the document is stored
        public DateTime UploadedAt { get; set; } // Timestamp of when the document was uploaded

        [MaxLength(300)]
        public string? Description { get; set; } // Optional description of the document 

        // Foreign key to Application
        public int ApplicationId { get; set; } //  [ForeignKey("ApplicationId")]
        public Application Application { get; set; } // Navigation property to Application


        public string UserAccountId { get; set; }

        public bool IsDeleted { get; set; } // Soft delete flag
    }
}
