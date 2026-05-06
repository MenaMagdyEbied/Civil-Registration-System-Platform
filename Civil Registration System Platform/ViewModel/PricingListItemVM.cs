namespace Civil_Registration_System_Platform.ViewModel
{
 
    
    public class PricingListItemVM
    {
        public int ServiceTypeId { get; set; }              
        public string ServiceTypeName { get; set; }           

        public int ApplicationTypeId { get; set; }            
        public string ApplicationTypeName { get; set; }       

        public int Price { get; set; }
        public string PriceDisplay { get; set; }              

        public int DurationInDays { get; set; }
        public string DurationDisplay { get; set; }            

        public string? Details { get; set; }                  
    }
}
