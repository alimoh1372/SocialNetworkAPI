namespace _00_Framework.Application;

    public class ApplicationMessage
    {
        public const string Duplication = "نمیتوان مورد تکراری ثبت کرد.";

        public const string NotFound = "موردی با مشخصات  درخواستی یافت نشد.";
        public const string OperationFailed = "عملیات با شکست مواجه شد";
        public const string PasswordsNotMatch = "پسورد و تکرار آن با هم مطابقت ندارند";
        public const string WrongUserPass = "نام کاربری یا کلمه رمز اشتباه است";
        public const string CantSelfRequest = "امکان درخواست دوستی به خود نمیباشد";
        public const string EditTimeOver = "time to edit is ended";
        public const string CantSendToNonFriendUsers = "You can't send message to non-friend users";
    }
