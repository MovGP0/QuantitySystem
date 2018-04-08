using QuantitySystem.Exceptions;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class ElectricalCurrent<T> : AnyQuantity<T>
    {
        public ElectricalCurrent() : base(1) { }

        public ElectricalCurrent(float exponent) : base(exponent) { }

        private static readonly QuantityDimension _Dimension = new QuantityDimension(0, 0, 0, 0, 1, 0, 0);
        public override QuantityDimension Dimension => _Dimension * Exponent;


        public static implicit operator ElectricalCurrent<T>(T value)
        {
            var Q = new ElectricalCurrent<T>
            {
                Value = value
            };

            return Q;
        }
    }
}
