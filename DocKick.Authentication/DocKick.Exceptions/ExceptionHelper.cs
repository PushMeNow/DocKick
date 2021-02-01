using System;
using DocKick.Helpers.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace DocKick.Exceptions
{
    public static class ExceptionHelper
    {
        public static void ThrowIfEmpty<TException>(object value)
            where TException : Exception, new()
        {
            if (value.IsEmpty())
            {
                throw new TException();
            }
        }
        
        public static void ThrowIfNotEmpty<TException>(object value)
            where TException : Exception, new()
        {
            if (!value.IsEmpty())
            {
                throw new TException();
            }
        }
        
        public static void ThrowArgumentNullIfEmpty(object value, string paramName)
        {
            if (value.IsEmpty())
            {
                throw new ArgumentNullException(paramName);
            }
        }
        
        public static void ThrowParameterNullIfEmpty(object value, string message)
        {
            if (value.IsEmpty())
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

        public static void ThrowNotFoundIfEmpty<T>(T value, string objName, string message = "{0} wasn't found.")
        {
            if (value.IsEmpty())
            {
                throw new ObjectNotFoundException(string.Format(message, objName));
            }
        }

        public static void ThrowParamInvalidIfNotEmpty(object value, string message)
        {
            if (!value.IsEmpty())
            {
                throw new ParameterInvalidException(message);
            }
        }
    }
}