using System;

namespace QuantitySystem.Exceptions
{
    public sealed class DimensionNotFoundException : QuantityExceptionBase
    {
        public DimensionNotFoundException()
        {
        }

        public DimensionNotFoundException(string message) : base(message)
        {
        }

        public DimensionNotFoundException(string message, Exception innerException) :
           base(message, innerException)
        {
        }
    }
}
