﻿using QuantitySystem.Exceptions;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Digital<T> : AnyQuantity<T>
    {

        public Digital() : base(1) { }

        public Digital(float exponent) : base(exponent) { }

        private static readonly QuantityDimension _Dimension = new QuantityDimension()
        {
            Digital = new DimensionDescriptors.DigitalDescriptor(1)
        };

        public override QuantityDimension Dimension => _Dimension * Exponent;


        public static implicit operator Digital<T>(T value)
        {
            var Q = new Digital<T>
            {
                Value = value
            };

            return Q;
        }
    }
}
