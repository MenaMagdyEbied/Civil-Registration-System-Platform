namespace Civil_Registration_System_Platform.Models
{
    public class ServicesTypeHelper
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ServicesTypeEnum { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ApplicationTypeEnum { get; set; }
        public int Price { get; set; }  
        public int MinDays { get; set; }
        public int MaxDays { get; set; }

        public string? Details { get; set; }
    }
}
