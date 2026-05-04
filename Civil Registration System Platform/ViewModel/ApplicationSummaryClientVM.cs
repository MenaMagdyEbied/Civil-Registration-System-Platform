namespace Civil_Registration_System_Platform.ViewModel
{
    public class ApplicationSummaryClientVM
    {
        public int ApplicationId { get; set; }
        public string ApplicationNumber { get; set; }
        public string ServiceTypeName { get; set; }  
        public string StatusName { get; set; }        
        public string StatusColor { get; set; }      
        public DateTime CreatedAt { get; set; }
        public string OfficeName { get; set; }
    }
}
