namespace Civil_Registration_System_Platform.ViewModel
{
    public class ServicesApplicationsVM
    {
        public ServiceType serviceType { get; set; }
        public List<AppDetail> appDetails { get; set; } = new List<AppDetail>();    
    }

    public class AppDetail
    {
        public ApplicationType applicationType { get; set; }
        public int Min {  get; set; }
        public int Max { get; set; }
        public int Price { get; set; }
        public string? Details { get; set; }
    }
}
