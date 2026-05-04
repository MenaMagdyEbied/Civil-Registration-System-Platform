using Civil_Registration_System_Platform.ViewModel.Application;

namespace Civil_Registration_System_Platform.ViewModel
{
    public class ReviewApplicationVM
    {
        public int ApplicationId { get; set; }
        public string ApplicationNumber { get; set; }
        public string ServiceTypeName { get; set; }
        public string ApplicationTypeName { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Price { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantNationalId { get; set; }
        public string ApplicantPhone { get; set; }
        public string ApplicantGovernorateName { get; set; }

        public string OfficeName { get; set; }

        public List<DocumentVM> Documents { get; set; }

        public List<TimelineItemVM> Timeline { get; set; }

        public string? AdminNotes { get; set; }
        public string? RejectionReason { get; set; }

        public DateTime? AppointmentDate { get; set; }
        public TimeSpan? AppointmentTime { get; set; }
    }
}
