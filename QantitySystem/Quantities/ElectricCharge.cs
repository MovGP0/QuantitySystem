﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class ElectricCharge<T> : DerivedQuantity<T>
    {
        public ElectricCharge()
            : base(1, new ElectricalCurrent<T>(), new Time<T>())
        {
        }

        public ElectricCharge(float exponent)
            : base(exponent, new ElectricalCurrent<T>(exponent), new Time<T>(exponent))
        {
        }


        public static implicit operator ElectricCharge<T>(T value)
        {
            var Q = new ElectricCharge<T>
            {
                Value = value
            };

            return Q;
        }


    }
}
