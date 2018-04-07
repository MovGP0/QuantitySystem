using System;

namespace QuantitySystem.Units
{
    public sealed class UnitsNotDimensionallyEqualException : UnitExceptionBase
    {
        public UnitsNotDimensionallyEqualException()
        {
        }

        public UnitsNotDimensionallyEqualException(string message) : base(message)
        {
        }

        public UnitsNotDimensionallyEqualException(string message, Exception innerException) :
           base(message, innerException)
        {
        }
    }
}
