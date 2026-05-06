namespace Civil_Registration_System_Platform.GlobalServices.GlobalInterface
{
    public interface IUserGlobalServices
    {
        Task<UserAccount> GetUser();

        /// <summary>يتحقق إن المستخدم الحالي أدمن — يرمي استثناء لو لأ</summary>
        Task<bool> CheckIfIsAdmin();

        /// <summary>يتحقق إن المستخدم الحالي قادر على مراجعة حسابات المواطنين (Admin أو AccountReviewer)</summary>
        Task<bool> CheckIfCanReviewAccounts();

        /// <summary>هل المستخدم الحالي AccountReviewer (وليس Admin)؟</summary>
        Task<bool> IsAccountReviewer();
    }
}
