using System;
using TechTalk.SpecFlow;

namespace OrkadWeb.Specs.Contexts
{
    [Binding]
    public class ExceptionContext
    {
        private Exception? exception;

        public Exception? HandleException()
        {
            var exception = this.exception;
            this.exception = null;
            return exception;
        }

        public void SetException(Exception exception)
        {
            this.exception = exception;
        }
    }
}
