using System;
using QuantitySystem.Units;

namespace QuantitySystem.Exceptions
{
    public sealed class MetricPrefixException : UnitExceptionBase
    {
        public MetricPrefixException()
        {
        }

        public MetricPrefixException(string message) : base(message)
        {
        }

        public MetricPrefixException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public double WrongExponent { get; set; }
        public MetricPrefix CorrectPrefix { get; set; }
        public double OverflowExponent { get; set; }
    }
}
