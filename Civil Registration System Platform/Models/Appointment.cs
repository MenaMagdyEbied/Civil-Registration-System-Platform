namespace Civil_Registration_System_Platform.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int Status { get; set; } // enum e.g., Scheduled, Completed, Cancelled
        [MaxLength(300)]
        public string? Note { get; set; } // Optional note about the appointment    

        // Foreign keys
        public int ApplicationId { get; set; } //[ForeignKey("ApplicationId")]
        public Application Application { get; set; } // Navigation property to Application



        //ScheduledById 
        public string ScheduledById { get; set; } //[ForeignKey("ScheduledById")]
        public UserAccount ScheduledBy { get; set; } // Navigation property to UserAccount

        public string UserAccountId { get; set; }
        public bool IsDeleted { get; set; } // Soft delete flag

    }
}
