using System;

namespace QuantitySystem.Exceptions
{
    public abstract class QuantityExceptionBase : Exception
    {
        protected QuantityExceptionBase()
        {
        }

        protected QuantityExceptionBase(string message) : base(message)
        {
        }

        protected QuantityExceptionBase(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}