namespace Civil_Registration_System_Platform.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }
        [StringLength(20, MinimumLength = 4)]
        public string ApplicationNumber { get; set; }   // unique 
        public int ServiceType { get; set; }   // enum e.g., Birth, Death, Marriage  
        public int? ApplicationType { get; set; }   // enum e.g., New, Correction, Replacement    
        public int Status { get; set; }   // enum e.g., Pending, Approved, Rejected 
        [MaxLength(300)]
        public string? Note { get; set; }  
        public DateTime CreatedAt { get; set; }  // default to current time when created 
        public DateTime? UpdatedAt { get; set; }  // automatic when the application was last updated  
        public DateTime? ReviewedAt { get; set; }  // when the application was reviewed (approved/rejected)


        // Foreign keys

        public int OfficeId { get; set; } //[ForeignKey("OfficeId")]
        public Office Office { get; set; } // Navigation property to Office


        public string UserAccountId { get; set; } // [ForeignKey("UserAccountId")]
        public UserAccount UserAccount { get; set; } // Navigation property to UserAccount

        public string? ReviewedById { get; set; } //[ForeignKey("ReviewedById")]
        public UserAccount? ReviewedUserAccount { get; set; } // Navigation property to UserAccount


        public List<ApplicationDocuments>? ApplicationDocuments { get; set; } // Navigation property to related documents
        public List<Appointment>? Appointments { get; set; } // Navigation property to related appointments 
        public List<TimelineEntry>? TimelineEntries { get; set; } // Navigation property to related timeline entries


        public bool IsDeleted { get; set; } // Soft delete flag

    }
}
