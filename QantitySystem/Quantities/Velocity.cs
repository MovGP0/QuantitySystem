using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Velocity<T> : DerivedQuantity<T>
    {
        public Velocity()
            : base(1, new Length<T>(), new Time<T>(-1))
        {
        }

        public Velocity(float exponent)
            : base(exponent, new Length<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator Velocity<T>(T value)
        {
            var Q = new Velocity<T>
            {
                Value = value
            };

            return Q;
        }

    }
}
