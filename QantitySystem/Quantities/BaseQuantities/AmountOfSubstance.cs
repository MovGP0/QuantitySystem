﻿using QuantitySystem.Exceptions;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class AmountOfSubstance<T> : AnyQuantity<T>
    {
        public AmountOfSubstance() : base(1) { }

        public AmountOfSubstance(float exponent) : base(exponent) { }


        private static readonly QuantityDimension _Dimension = new QuantityDimension(0, 0, 0, 0, 0, 1, 0);
        public override QuantityDimension Dimension => _Dimension * Exponent;

        public static implicit operator AmountOfSubstance<T>(T value)
        {
            var Q = new AmountOfSubstance<T>
            {
                Value = value
            };

            return Q;
        }


    }
}
