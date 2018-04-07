using System;

namespace QuantitySystem.Units
{
    public sealed class UnitException : UnitExceptionBase
    {
        public UnitException()
        {
        }

        public UnitException(string message) : base(message)
        {
        }

        public UnitException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
