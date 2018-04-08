using System;

namespace QuantitySystem.Exceptions
{
    public abstract class UnitExceptionBase : QuantityExceptionBase
    {
        protected UnitExceptionBase()
        {
        }

        protected UnitExceptionBase(string message) : base(message)
        {
        }

        protected UnitExceptionBase(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}