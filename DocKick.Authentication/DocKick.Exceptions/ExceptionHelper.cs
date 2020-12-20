using System;

namespace DocKick.Exceptions
{
    public static class ExceptionHelper
    {
        public static void ThrowExternalAuthentication()
        {
            throw new ExternalAuthenticationException();
        }

        public static void ThrowIfNull<TException>(object value)
            where TException : Exception, new()
        {
            if (value is null)
            {
                throw new TException();
            }
        }
        
        public static void ThrowIfNull(object value, string paramName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void ThrowIfTrue<TException>(bool condition)
            where TException : Exception, new()
        {
            if (condition)
            {
                throw new TException();
            }
        }
    }
}