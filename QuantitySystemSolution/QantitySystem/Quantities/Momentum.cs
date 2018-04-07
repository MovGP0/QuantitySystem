using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Momentum<T> : DerivedQuantity<T>
    {
        public Momentum()
            : base(1, new Mass<T>(), new Velocity<T>())
        {
        }

        public Momentum(float exponent)
            : base(exponent, new Mass<T>(exponent), new Velocity<T>(exponent))
        {
        }


        public static implicit operator Momentum<T>(T value)
        {
            var Q = new Momentum<T>
            {
                Value = value
            };

            return Q;
        }

    }
}

