using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OrkadWeb.Services
{
    public static class HashUtils
    {
        /// <summary>
        /// Permet de crypter en SHA256
        /// </summary>
        /// <param name="value">la valeur a crypter</param>
        /// <returns>la valeur crypter</returns>
        public static string HashSHA256(string value)
        {
            StringBuilder Sb = new StringBuilder();
            using (var hash = SHA256.Create())
            {
                foreach (var b in hash.ComputeHash(Encoding.UTF8.GetBytes(value)))
                {
                    Sb.Append(b.ToString("x2"));
                }
            }
            return Sb.ToString();
        }
    }
}
