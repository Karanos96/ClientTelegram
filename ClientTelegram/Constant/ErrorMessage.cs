namespace ClientTelegram.Constant
{
    public static class ErrorMessage
    {
        public const string ERROR_OPTION_TELEGRAM = "Telegram Options was null. Please look your appsettings.json for a correct configuration.";
        public const string ERROR_PHONENUMBER = "Phonenumber is required.";
        public const string ERROR_PHONENUMBER_ALREADY_REGISTER = "This number is already connected.";
        public const string ERROR_ACCESS_CODE = "Access code is required.";
        public const string ERROR_AUTHENTICATION_ACCESS_CODE = "Access code is not valid.";
        public const string ERROR_AUTHENTICATION_PHONENUMBER = "Error while connecting with phonenumber. Try again.";
        public const string ERROR_LIMIT_CHATS = "Limit of record is required.";
        public const string ERROR_GET_INFO_CHATS = "An error incurred while get chat info.";
        public const string ERROR_SESSION_NOT_FOUND = "Not found session with id: ";
        public const string ERROR_SESSION_ID_NOT_VALID = "Session Id not valid.";



    }
}
