using Civil_Registration_System_Platform.Enums;

namespace Civil_Registration_System_Platform.Helpers
{
    public static class EnumExtensions
    {
        public static string ToArabicName(this ServiceType serviceType) =>
            serviceType switch
            {
                ServiceType.BirthCertificate => "شهادة الميلاد",
                ServiceType.DeathCertificate => "شهادة الوفاة",
                ServiceType.MarriageCertificate => "قسيمة الزواج",
                ServiceType.DivorceCertificate => "قسيمة الطلاق",
                ServiceType.NationalId => "بطاقة الرقم القومي",
                ServiceType.Passport => "جواز السفر",
                ServiceType.CriminalRecord => "صحيفة السوابق",
                ServiceType.FamilyCard => "قيد عائلي",
                ServiceType.IndividualRecord => "قيد فردي",
                ServiceType.DriversLicense => "رخصة القيادة",
                ServiceType.CustomDocument => "مستند مخصص",
                _ => "غير معروف"
            };

        public static string ToArabicName(this ApplicationType appType) =>
            appType switch
            {
                ApplicationType.New => "جديد",
                ApplicationType.Renewal => "تجديد",
                ApplicationType.Replacement => "بدل فاقد",
                ApplicationType.Damaged => "بدل تالف",
                ApplicationType.Correction => "تصحيح بيانات",
                _ => "غير معروف"
            };

        public static string ToArabicName(this ApplicationStatus status) =>
            status switch
            {
                ApplicationStatus.Submitted => "تم التقديم",
                ApplicationStatus.UnderReview => "قيد المراجعة",
                ApplicationStatus.AdditionalInfoRequired => "مطلوب مستندات إضافية",
                ApplicationStatus.AppointmentScheduled => "تم تحديد موعد",
                ApplicationStatus.MedicalExamPending => "بانتظار الكشف الطبي",
                ApplicationStatus.TheoryTestPending => "بانتظار الاختبار النظري",
                ApplicationStatus.PracticalTestPending => "بانتظار الاختبار العملي",
                ApplicationStatus.Approved => "تمت الموافقة",
                ApplicationStatus.Rejected => "مرفوض",
                ApplicationStatus.Issued => "تم الإصدار",
                ApplicationStatus.Cancelled => "ملغي",
                _ => "غير معروف"
            };

        public static string ToStatusColor(this ApplicationStatus status) =>
            status switch
            {
                ApplicationStatus.Submitted => "#1565c0",
                ApplicationStatus.UnderReview => "#c8a415",
                ApplicationStatus.AdditionalInfoRequired => "#e65100",
                ApplicationStatus.AppointmentScheduled => "#6a1b9a",
                ApplicationStatus.MedicalExamPending => "#e65100",
                ApplicationStatus.TheoryTestPending => "#e65100",
                ApplicationStatus.PracticalTestPending => "#e65100",
                ApplicationStatus.Approved => "#2e7d32",
                ApplicationStatus.Rejected => "#c41e3a",
                ApplicationStatus.Issued => "#1a7a3a",
                ApplicationStatus.Cancelled => "#6c757d",
                _ => "#6c757d"
            };

        public static string ToArabicName(this AppointmentStatus status) =>
            status switch
            {
                AppointmentStatus.Scheduled => "مجدول",
                AppointmentStatus.Confirmed => "مؤكد",
                AppointmentStatus.Completed => "مكتمل",
                AppointmentStatus.Cancelled => "ملغي",
                AppointmentStatus.NoShow => "لم يحضر",
                AppointmentStatus.Rescheduled => "تم إعادة الجدولة",
                _ => "غير معروف"
            };
    }
}
