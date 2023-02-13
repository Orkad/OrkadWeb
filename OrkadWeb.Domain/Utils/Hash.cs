using System;
using System.Security.Cryptography;
using System.Text;

namespace OrkadWeb.Domain.Utils
{
    /// <summary>
    /// Secured Hash function for the whole app
    /// </summary>
    public static class Hash
    {
        // changing theses values will invalid every previous generated hashes
        public const int SALT_LENGTH = 32;
        public const int SHA256_LENGTH = 64;

        /// <summary>
        /// Obtain random Salt
        /// </summary>
        public static string Salt()
        {
            byte[] bytes = new byte[SALT_LENGTH / 2];
            using var gen = RandomNumberGenerator.Create();
            gen.GetBytes(bytes);
            return BytesToString(bytes);
        }

        /// <summary>
        /// Sha256 hashing function
        /// </summary>
        /// <param name="value">value to hash</param>
        public static string Sha256(string value)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));
            return BytesToString(bytes);
        }

        /// <summary>
        /// Create a new secured hash
        /// </summary>
        /// <param name="value">clear value to hash</param>
        public static string Create(string value)
        {
            var salt = Salt();
            return Sha256(value + salt) + salt;
        }

        /// <summary>
        /// Validate that value correspond to secured hash created with <see cref="Create(string)"/> 
        /// </summary>
        /// <param name="value">clear value to check</param>
        /// <param name="hash">hash to compare</param>
        public static bool Validate(string value, string hash)
        {
            if (value == null || hash == null || hash.Length != SALT_LENGTH + SHA256_LENGTH)
            {
                return false;
            }
            var salt = hash.Substring(SHA256_LENGTH, SALT_LENGTH);
            return Sha256(value + salt) + salt == hash;
        }

        /// <summary>
        /// Converts byte array into string representation
        /// </summary>
        /// <param name="bytes">array to convert</param>
        private static string BytesToString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
