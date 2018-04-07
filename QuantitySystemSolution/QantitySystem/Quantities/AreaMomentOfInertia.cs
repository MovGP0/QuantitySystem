using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class AreaMomentOfInertia<T> : DerivedQuantity<T>
    {

        public AreaMomentOfInertia()
            : base(1, new Length<T>(2, LengthType.Regular), new Length<T>(2, LengthType.Polar))
        {
        }

        public AreaMomentOfInertia(float exponent)
            : base(exponent, new Length<T>(2 * exponent, LengthType.Regular), new Length<T>(2 * exponent, LengthType.Polar))
        {
        }


        public static implicit operator AreaMomentOfInertia<T>(T value)
        {
            var Q = new AreaMomentOfInertia<T>
            {
                Value = value
            };

            return Q;
        }


    }
}
