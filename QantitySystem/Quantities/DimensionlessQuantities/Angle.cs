using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities.DimensionlessQuantities
{
    public class Angle<T> : DerivedQuantity<T>
    {

        public Angle()
            : base(1, new Length<T>(1, LengthType.Regular), new Length<T>(-1, LengthType.Polar))
        {
        }

        public Angle(float exponent)
            : base(exponent, new Length<T>(exponent, LengthType.Regular), new Length<T>(-1 * exponent, LengthType.Polar))
        {
        }


        public static implicit operator Angle<T>(T value)
        {
            var Q = new Angle<T>
            {
                Value = value
            };

            return Q;
        }


    }
}
