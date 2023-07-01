namespace _00_Framework.Application
{
    /// <summary>
    /// ready message to use in validating 
    /// </summary>
    public  static class ValidatingMessage
    {
        public const  string IsRequired= "برای ادامه لطفا این فیلد را پر نمائید.";

        public const string MaxLength = "تعداد کاراکتر وارد شده بیش از مقدار مجاز میباشد.";
        public const string MaxFileSize = "حجم فایل ارسالی بیشتر از حد مجاز میباشد.";
        public const string FileExtension = "فرمت فایل ارسالی مجاز نمیباشد.";
        public const string NumberOutOfRange = "عدد وارد شده خارج از محدوده مجاز میباشد";
        public const string ForbiddenToAccess = "Access to this content is forbidden to you";



    }
}