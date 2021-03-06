﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Inductance <T>  : DerivedQuantity<T>
    {
        public Inductance()
            : base(1, new MagneticFlux<T>(), new ElectricalCurrent<T>(-1))
        {
        }

        public Inductance(float exponent)
            : base(exponent, new MagneticFlux<T>(exponent), new ElectricalCurrent<T>(-1 * exponent))
        {
        }


        public static implicit operator Inductance<T>(T value)
        {
            var Q = new Inductance<T>
            {
                Value = value
            };

            return Q;
        }

    }
}
