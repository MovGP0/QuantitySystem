using System;
using QuantitySystem.Exceptions;

namespace QuantitySystem.Units
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