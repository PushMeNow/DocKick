using System;

namespace DocKick.Exceptions
{
    public class ParameterNullException : Exception
    {
        public ParameterNullException(string message) : base(message) { }
    }
}