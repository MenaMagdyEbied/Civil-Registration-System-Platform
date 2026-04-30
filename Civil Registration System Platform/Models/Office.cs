
namespace Civil_Registration_System_Platform.Models
{
    public class Office
    {
        [Key]
        public int OfficeId { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(11, MinimumLength = 11)]
        [RegularExpression(@"^01[0-2,5][0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number")]
        public string Phone { get; set; }        
        public bool IsActive { get; set; }
       

        public int GovernorateId { get; set; }       
        public Governorate Governorate { get; set; } //   [ForeignKey("GovernorateId")]  



        public List<Application>? Applications { get; set; } // Navigation property to Applications

        public List<UserAccount>? UserAccounts { get; set; } //[InverseProperty("Office")]        
        public List<UserAccount>? ManageUserAccounts { get; set; }//[InverseProperty("ManageOffice")]
    }
}
