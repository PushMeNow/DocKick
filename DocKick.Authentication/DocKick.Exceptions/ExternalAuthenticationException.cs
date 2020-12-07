using System;

namespace DocKick.Exceptions
{
    public class ExternalAuthenticationException : Exception
    {
        public ExternalAuthenticationException() : base($"External authentication error.")
        {
        }
    }
}