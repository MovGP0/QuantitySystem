using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{

    /// <summary>
    /// dV = r  dz dr d\theta
    /// dV = PL RL PL RL/PL = PL RL RL    
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CylindricalVolume<T> : DerivedQuantity<T>
    {
        public CylindricalVolume()
            : base(1, new PolarLength<T>(), new Length<T>(2))
        {
        }

        public CylindricalVolume(float exponent)
            : base(exponent, new PolarLength<T>(), new Length<T>(2 * exponent))
        {
        }


        public static implicit operator CylindricalVolume<T>(T value)
        {
            var Q = new CylindricalVolume<T>
            {
                Value = value
            };

            return Q;
        }
    }
}
