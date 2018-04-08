using System;

namespace QuantitySystem.Exceptions
{
    public sealed class QuantityNotFoundException : QuantityExceptionBase
    {
        public QuantityNotFoundException()
        {
        }

        public QuantityNotFoundException(string message) : base(message)
        {
        }

        public QuantityNotFoundException(string message, Exception innerException) :
           base(message, innerException)
        {
        }
    }
}
