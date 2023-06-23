namespace _00_Framework.Application;

    public class ApplicationMessage
    {
        public const string Duplication = "نمیتوان مورد تکراری ثبت کرد.";

        public const string NotFound = "موردی با مشخصات  درخواستی یافت نشد.";
        public const string OperationFailed = "عملیات با شکست مواجه شد";
        public static string PasswordsNotMatch = "پسورد و تکرار آن با هم مطابقت ندارند";
        public static string WrongUserPass = "نام کاربری یا کلمه رمز اشتباه است";
        public static string CantSelfRequest = "امکان درخواست دوستی به خود نمیباشد";
        public static string EditTimeOver = "time to edit is ended";
    }
