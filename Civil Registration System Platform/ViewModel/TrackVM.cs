
namespace Civil_Registration_System_Platform.ViewModel.Application

{
    public class TrackVM
    {
        [Required(ErrorMessage = "أدخل رقم الطلب")]
        public string ApplicationNumber { get; set; }

        public ApplicationTrackResultVM? Result { get; set; }
    }
}