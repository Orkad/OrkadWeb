using System;
using System.Runtime.Serialization;

namespace OrkadWeb.Services.Exceptions
{
    [Serializable]
    public class NotFoundException<T> : Exception
    {
        public NotFoundException(object id) : base($"Impossible de trouver l'entitée ({typeof(T)}) correspondant à l'identifiant ({id})") { }
        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
