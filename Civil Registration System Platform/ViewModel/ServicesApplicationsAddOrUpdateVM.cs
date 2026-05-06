namespace Civil_Registration_System_Platform.ViewModel
{
    public class ServicesApplicationsAddOrUpdateVM
    {
        public int ServicesTypeEnum { get; set; }
        public int ApplicationTypeEnum { get; set; }
        public int Price { get; set; }
        public int min {  get; set; }
        public int max { get; set; }
        public string? Details { get; set; }
    }
}
