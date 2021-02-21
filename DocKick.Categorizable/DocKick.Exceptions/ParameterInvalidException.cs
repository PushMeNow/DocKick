using System;

namespace DocKick.Exceptions
{
    public class ParameterInvalidException : Exception
    {
        public ParameterInvalidException(string message) : base(message) { }
    }
}