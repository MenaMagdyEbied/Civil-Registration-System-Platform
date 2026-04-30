namespace Civil_Registration_System_Platform.Models
{
    public class ServicesTypeHelper
    {
        [Key]
        public int ServicesTypeEnum { get; set; }   
        public int Price { get; set; }  
        public int DurationInDays { get; set; }
        [MaxLength(2000)]
        public string? Details { get; set; } 

    }
}
