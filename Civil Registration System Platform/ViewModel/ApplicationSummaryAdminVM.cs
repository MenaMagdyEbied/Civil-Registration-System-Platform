namespace Civil_Registration_System_Platform.ViewModel
{
    public class ApplicationSummaryAdminVM
    {
        public int ApplicationId { get; set; }
        public string ApplicationNumber { get; set; }
        public string ApplicantName { get; set; }     
        public string ApplicantNationalId { get; set; }
        public string ServiceTypeName { get; set; }
        public string ApplicationTypeName { get; set; }
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
