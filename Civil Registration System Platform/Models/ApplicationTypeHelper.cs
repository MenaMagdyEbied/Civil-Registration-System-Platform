namespace Civil_Registration_System_Platform.Models
{
    public class ApplicationTypeHelper
    {
        [Key]
        public int ApplicationTypeEnum { get; set; }   
        public int Price { get; set; }  
        public int DurationInDays { get; set; }
        [MaxLength(2000)]
        public string? Details { get; set; }
    }
}
