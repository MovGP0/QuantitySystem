﻿namespace QuantitySystem.Quantities
{
    public class Capacitance<T> : DerivedQuantity<T>
    {
        public Capacitance()
            : base(1, new ElectricCharge<T>(), new ElectromotiveForce<T>(-1))
        {
        }

        public Capacitance(float exponent)
            : base(exponent, new ElectricCharge<T>(exponent), new ElectromotiveForce<T>(-1 * exponent))
        {
        }


        public static implicit operator Capacitance<T>(T value)
        {
            var Q = new Capacitance<T>
            {
                Value = value
            };

            return Q;
        }


    }

}
