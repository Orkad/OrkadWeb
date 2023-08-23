using System;

namespace OrkadWeb.Specs.Contexts
{
    [Binding]
    public class ExceptionContext
    {
        private Exception pendingException;

        public Exception HandleException()
        {
            var exception = pendingException;
            pendingException = null;
            return exception;
        }

        public void SetException(Exception exception)
        {
            pendingException = exception;
        }
    }
}
