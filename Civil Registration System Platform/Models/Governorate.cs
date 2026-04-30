namespace Civil_Registration_System_Platform.Models
{
    public class Governorate
    {
        [Key]
        public int GovernorateId { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        [StringLength(20, MinimumLength = 4)]
        public string Code { get; set; }        
        public bool IsActive { get; set; }      



        public List<Office>? Offices { get; set; } // Navigation property to Offices
        public List<UserAccount>? UserAccounts { get; set; } // Navigation property to RegularUsers
    }


}
