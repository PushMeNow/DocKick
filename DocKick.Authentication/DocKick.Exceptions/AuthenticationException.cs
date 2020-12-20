using System;

namespace DocKick.Exceptions
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException() : base("Authentication error.")
        {
            
        }
    }
}