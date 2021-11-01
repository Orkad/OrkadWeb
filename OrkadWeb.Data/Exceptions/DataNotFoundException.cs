using System;
using System.Runtime.Serialization;

namespace OrkadWeb.Data.Exceptions
{
    [Serializable]
    public class DataNotFoundException<T> : Exception
    {
        public DataNotFoundException(object id) : base($"Impossible de trouver l'entitée ({typeof(T)}) correspondant à l'identifiant ({id})") { }
        protected DataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
