using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Civil_Registration_System_Platform.Models
{
    public class UserAccount : IdentityUser
    {
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; }
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression(@"^01[0-2,5][0-9]{8}$", ErrorMessage = "رقم الموبايل المصري غير صحيح")]
        public string EGPhoneNumber { get; set; }
        [StringLength(14, MinimumLength = 14)]
        public string NationalID { get; set; }
        public int Gender { get; set; } // enum
        public int MaritalStatus { get; set; } // enum
        public bool IsConfirmed { get; set; }
        public string CardImagePath { get; set; }

        public DateOnly CreatedAt { get; set; }


        public bool IsRejected { get; set; } 
        public string? RejectionMessage { get; set; }


        public int? GovernorateId { get; set; }        
        public Governorate? Governorate { get; set; } // [ForeignKey("GovernorateId")]  

        public int? OfficeId { get; set; }       
        public Office? Office { get; set; } //  [ForeignKey("OfficeId")] 

         
        public int? ManageOfficeId { get; set; }        
        public Office? ManageOffice { get; set; } //[ForeignKey("ManageOfficeId")] 
        
        public List<Application>? ApplicationsApply { get; set; } // [InverseProperty("UserAccount")]     
        public List<Application>? ApplicationsReviewed { get; set; } //  [InverseProperty("ReviewedUserAccount")]      
        public List<Appointment>? Appointments { get; set; } //  [InverseProperty("ScheduledBy")]      
        public List<TimelineEntry>? TimelineEntries { get; set; } //  [InverseProperty("PerformedBy")] 
    }
}
