using QuantitySystem.Exceptions;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Temperature<T> : AnyQuantity<T>
    {
        public Temperature() : base(1) { }

        public Temperature(float exponent) : base(exponent) { }

        private static readonly QuantityDimension _Dimension = new QuantityDimension(0, 0, 0, 1, 0, 0, 0);
        public override QuantityDimension Dimension => _Dimension * Exponent;


        public static implicit operator Temperature<T>(T value)
        {
            var Q = new Temperature<T>
            {
                Value = value
            };

            return Q;
        }
    }
}
