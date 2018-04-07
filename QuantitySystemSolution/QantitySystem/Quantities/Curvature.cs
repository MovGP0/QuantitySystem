using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Curvature<T> : DerivedQuantity<T>
    {
        public Curvature()
            : base(1, new PolarLength<T>(-1))
        {
        }

        public Curvature(float exponent)
            : base(exponent, new PolarLength<T>(-1 * exponent))
        {
        }


        public static implicit operator Curvature<T>(T value)
        {
            var Q = new Curvature<T>
            {
                Value = value
            };

            return Q;
        }
    }
}
