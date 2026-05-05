namespace Civil_Registration_System_Platform.Enums
{
    [Flags]
    public enum ApplicationType
    {
        New = 1,
        Renewal = 2,
        Replacement = 4,   
        Damaged = 8,        
        Correction = 16      
    }
}
