namespace MonitoringIt
{
    using System;

    public class InitializeResult
    {
        public InitializeResult()
        {
        }

        public InitializeResult(Exception exception)
        {
            this.Exception = exception;
        }

        public bool Success
        {
            get
            {
                return this.Exception != null;
            }
        }

        public Exception Exception
        {
            get;
            private set;
        }
    }
}