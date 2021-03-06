﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class MassMomentOfInertia<T> : DerivedQuantity<T>
    {
        public MassMomentOfInertia()
            : base(1, new Mass<T>(), new PolarLength<T>(2))
        {
        }

        public MassMomentOfInertia(float exponent)
            : base(exponent, new Mass<T>(exponent), new PolarLength<T>(2 * exponent))
        {
        }


        public static implicit operator MassMomentOfInertia<T>(T value)
        {
            var Q = new MassMomentOfInertia<T>
            {
                Value = value
            };

            return Q;
        }

    }
}
