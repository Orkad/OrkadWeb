using System;

namespace OrkadWeb.Services.Exceptions
{
    /// <summary>
    /// Classe de base pour les exceptions métiers
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {

        }
    }
}
