using System;

namespace DocKick.Exceptions
{
    public static class ExceptionHelper
    {
        public static void ThrowExternalAuthentication()
        {
            throw new ExternalAuthenticationException();
        }

        public static void ThrowIfNull<TException>(object @value)
            where TException : Exception, new()
        {
            throw new TException();
        }
    }
}