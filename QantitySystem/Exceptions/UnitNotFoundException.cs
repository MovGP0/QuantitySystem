using System;

namespace QuantitySystem.Exceptions
{
    public sealed class UnitNotFoundException : UnitExceptionBase
    {
        public UnitNotFoundException()
        {
        }

        public UnitNotFoundException(string unit)
            : base(unit + " Not found")
        {
        }

        public UnitNotFoundException(string message, string unit)
            : base(unit + " " + message)
        {
        }

        public UnitNotFoundException(string message, Exception innerException) :
           base(message, innerException)
        {
        }
    }
}
