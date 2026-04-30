namespace Civil_Registration_System_Platform.Models
{
    public class TimelineEntry
    {
        [Key]
        public int TimelineEntryId { get; set; }
        
        public int Status { get; set; } // enum e.g., Submitted, Under Review, Approved, Rejected   
        public DateTime Timestamp { get; set; } // When the event occurred

        [MaxLength(300)]
        public string? Description { get; set; } // Description of the timeline entry

        // Foreign key to Application
        public int ApplicationId { get; set; }
        
        public Application Application { get; set; } // [ForeignKey("ApplicationId")] 


        //PerformedBy
        public string PerformedById { get; set; }        
        public UserAccount PerformedBy { get; set; } //[ForeignKey("PerformedById")] 

        public string UserAccountId { get; set; }
        public bool IsDeleted { get; set; } // Soft delete flag
    }
}
