using System;

namespace QuantitySystem.Exceptions
{
    public sealed class QuantitiesNotDimensionallyEqualException : QuantityExceptionBase
    {
        public QuantitiesNotDimensionallyEqualException()
        {
        }

        public QuantitiesNotDimensionallyEqualException(string message): base(message) 
        {
        }

        public QuantitiesNotDimensionallyEqualException(string message, Exception innerException): 
            base (message, innerException)
        {
        }
    }
}
