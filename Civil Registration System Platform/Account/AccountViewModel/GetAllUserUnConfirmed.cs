namespace Civil_Registration_System_Platform.Account.AccountViewModel
{
    public class GetAllUserUnConfirmed
    {
       
        public string UserId { get; set; }  
        public string FullName { get; set; }
        public string EGPhoneNumber { get; set; }
        public string NationalID { get; set; }
        public int Gender { get; set; } // enum
        public int MaritalStatus { get; set; } // enum
        public string CardImagePath { get; set; }
        public string Email { get; set; }
        public DateOnly CreatedAt { get; set; } 
        public string GovernorateName { get; set; }
        public string OfficeName { get; set; }
    }
}
