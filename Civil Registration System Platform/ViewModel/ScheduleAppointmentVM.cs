using Civil_Registration_System_Platform.Enums;

namespace Civil_Registration_System_Platform.ViewModel
{
    
    public class ScheduleAppointmentVM
    {
        [Required(ErrorMessage = "رقم الطلب مطلوب")]
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "تاريخ الموعد مطلوب")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "وقت الموعد مطلوب")]
        public TimeSpan AppointmentTime { get; set; }

      
        public ApplicationStatus? TargetStatus { get; set; }

        [MaxLength(300)]
        public string? Note { get; set; }
    }
}
