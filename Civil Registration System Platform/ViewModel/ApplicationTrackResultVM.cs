using Civil_Registration_System_Platform.ViewModel.Application;

namespace Civil_Registration_System_Platform.ViewModel
{
    public class ApplicationTrackResultVM
    {
        public string ApplicationNumber { get; set; }
        public string ServiceTypeName { get; set; }    
        public string ApplicationTypeName { get; set; }
        public string StatusName { get; set; }         
        public string StatusColor { get; set; }        
        public DateTime CreatedAt { get; set; }
        public string OfficeName { get; set; }
        public int Price { get; set; }

        public List<TimelineItemVM> Timeline { get; set; }
    }
}
