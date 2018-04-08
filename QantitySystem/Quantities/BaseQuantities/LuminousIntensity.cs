using QuantitySystem.Exceptions;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class LuminousIntensity<T> : AnyQuantity<T>
    {
        public LuminousIntensity() : base(1) { }

        public LuminousIntensity(float exponent) : base(exponent) { }

        private static readonly QuantityDimension _Dimension = new QuantityDimension(0, 0, 0, 0, 0, 0, 1);
        public override QuantityDimension Dimension => _Dimension * Exponent;


        public static implicit operator LuminousIntensity<T>(T value)
        {
            var Q = new LuminousIntensity<T>
            {
                Value = value
            };

            return Q;
        }

    }
}
