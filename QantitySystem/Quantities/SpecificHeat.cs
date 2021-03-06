﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    /// <summary>
    /// Energy per unit mass x temperature
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpecificHeat<T> : DerivedQuantity<T>
    {

        public SpecificHeat()
            : base(1, new Energy<T>(), new Mass<T>(-1), new Temperature<T>(-1))
        {
        }


        public SpecificHeat(float exponent)
            : base(exponent, new Energy<T>(exponent), new Mass<T>(-1 * exponent), new Temperature<T>(-1 * exponent))
        {
        }


        public static implicit operator SpecificHeat<T>(T value)
        {
            var Q = new SpecificHeat<T>
            {
                Value = value
            };

            return Q;
        }

    }
}
