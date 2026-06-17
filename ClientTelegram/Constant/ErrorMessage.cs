namespace ClientTelegram.Constant
{
    public static class ErrorMessage
    {
        public const string ERROR_OPTION_TELEGRAM = "Telegram Options was null. Please look your appsettings.json for a correct configuration.";
        public const string ERROR_PHONENUMBER = "Phonenumber is required.";
        public const string ERROR_ACCESS_CODE = "Access code is required.";
        public const string ERROR_AUTHENTICATION_ACCESS_CODE = "Access code is not valid.";
        public const string ERROR_AUTHENTICATION_PHONENUMBER = "Error while connecting with phonenumber. Try again.";
        public const string ERROR_LIMIT_CHATS = "Limit of record is required.";
        public const string ERROR_GET_INFO_CHATS = "An error incurred while get chat info.";


    }
}
