namespace OrkadWeb.Logic.Config.Queries
{
    /// <summary>
    /// Global confiration result
    /// </summary>
    public class GlobalConfigurationResult
    {
        /// <summary>
        /// Minimum username length for registration
        /// </summary>
        public int UsernameMinLength { get; set; }
        /// <summary>
        /// Maximum username length for registration
        /// </summary>
        public int UsernameMaxLength { get; set; }

        /// <summary>
        /// Username Regex acceptation for registration
        /// </summary>
        public string UsernameRegex { get; set; }

        /// <summary>
        /// Minimum password length for registration
        /// </summary>
        public int PasswordMinLength { get; set; }

        /// <summary>
        /// Maximum password length for registration
        /// </summary>
        public int PasswordMaxLength { get; set; }

        /// <summary>
        /// Password Regex acceptation for registration
        /// </summary>
        public string PasswordRegex { get; set; }

        /// <summary>
        /// Regex for emails
        /// </summary>
        public string EmailRegex { get; set; }
    }
}
