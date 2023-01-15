namespace OrkadWeb.Logic.Config
{
    /// <summary>
    /// Application error code list
    /// </summary>
    public static class ErrorCodes
    {
        public const string USERNAME_REQUIRED = "USERNAME_REQUIRED";
        public const string USERNAME_TOO_SHORT = "USERNAME_TOO_SHORT";
        public const string USERNAME_TOO_LONG = "USERNAME_TOO_LONG";
        public const string USERNAME_WRONG_FORMAT = "USERNAME_WRONG_FORMAT";
        public const string USERNAME_ALREADY_EXISTS = "USERNAME_ALREADY_EXISTS";

        public const string PASSWORD_REQUIRED = "PASSWORD_REQUIRED";
        public const string PASSWORD_TOO_SHORT = "PASSWORD_TOO_SHORT";
        public const string PASSWORD_TOO_LONG = "PASSWORD_TOO_LONG";
        public const string PASSWORD_WRONG_FORMAT = "PASSWORD_WRONG_FORMAT";

        public const string EMAIL_REQUIRED = "EMAIL_REQUIRED";
        public const string EMAIL_WRONG_FORMAT = "EMAIL_WRONG_FORMAT";
        public const string EMAIL_ALREADY_EXISTS = "EMAIL_ALREADY_EXISTS";
    }

}
