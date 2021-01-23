using System;
using Microsoft.IdentityModel.Tokens;

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
        
        public static void ThrowIfNotNull<TException>(object value)
            where TException : Exception, new()
        {
            if (value is not null)
            {
                throw new TException();
            }
        }
        
        public static void ThrowArgumentNullIfNull(object value, string paramName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
        
        public static void ThrowParameterNullIfNull(object value, string message)
        {
            if (value is null)
            {
                throw new ParameterNullException(message);
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

        public static void ThrowParameterInvalidIfTrue(bool condition, string message)
        {
            if (condition)
            {
                throw new ParameterInvalidException(message);
            }
        }

        public static void ThrowNotFoundIfNull<T>(T value, string objName, string message = "{0} wasn't found.")
        {
            if (value is null)
            {
                throw new ObjectNotFoundException(string.Format(message, objName));
            }
        }

        public static void ThrowParamInvalidIfNotNull(object value, string message)
        {
            if (value is not null)
            {
                throw new ParameterInvalidException(message);
            }
        }
    }
}