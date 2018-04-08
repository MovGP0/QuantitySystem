using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Torque<T> : DerivedQuantity<T>
    {
        public Torque()
            : base(1, new Force<T>(), new Length<T>(1, LengthType.Polar))
        {
        }

        public Torque(float exponent)
            : base(exponent, new Force<T>(exponent), new Length<T>(exponent, LengthType.Polar))
        {
        }


        public static implicit operator Torque<T>(T value)
        {
            var Q = new Torque<T>
            {
                Value = value
            };

            return Q;
        }

    }
}
