using System;

namespace QuantitySystem.Exceptions
{
    public sealed class QuantityException : QuantityExceptionBase
    {
        public QuantityException()
        {
        }

        public QuantityException(string message) : base(message)
        {
        }

        public QuantityException(string message, Exception innerException) :
           base(message, innerException)
        {
        }
    }
}
