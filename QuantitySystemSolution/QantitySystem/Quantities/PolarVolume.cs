using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PolarVolume<T> : DerivedQuantity<T>
    {
        public PolarVolume()
            : base(1, new PolarLength<T>(3))
        {
        }

        public PolarVolume(float exponent)
            : base(exponent, new PolarLength<T>(3 * exponent))
        {
        }


        public static implicit operator PolarVolume<T>(T value)
        {
            var Q = new PolarVolume<T>
            {
                Value = value
            };

            return Q;
        }
    }
}
