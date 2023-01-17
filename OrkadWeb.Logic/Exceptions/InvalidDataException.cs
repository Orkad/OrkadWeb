using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Logic.Exceptions
{

    [Serializable]
    public class InvalidDataException : Exception
    {
        public InvalidDataException() { }
        public InvalidDataException(string message) : base(message) { }
        public InvalidDataException(string message, Exception inner) : base(message, inner) { }
        protected InvalidDataException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
