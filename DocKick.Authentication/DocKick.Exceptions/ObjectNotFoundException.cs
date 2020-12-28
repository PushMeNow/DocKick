using System;

namespace DocKick.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string objName) : base($"{objName} wasn't found.")
        {
        }
    }
}