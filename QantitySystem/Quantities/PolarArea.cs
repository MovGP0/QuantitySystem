using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class PolarArea<T> : DerivedQuantity<T>
    {
        public PolarArea()
            : base(1, new PolarLength<T>(2))
        {
        }

        public PolarArea(float exponent)
            : base(exponent, new PolarLength<T>(2 * exponent))
        {
        }


        public static implicit operator PolarArea<T>(T value)
        {
            var Q = new PolarArea<T>
            {
                Value = value
            };

            return Q;
        }


    }
}
