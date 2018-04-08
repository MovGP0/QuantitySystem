using QuantitySystem.Exceptions;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Mass<T> : AnyQuantity<T>
    {
        /// <summary>
        /// Create Mass object with dimension equals 1 M^1.
        /// </summary>
        public Mass() : base(1) 
        {
            
        }

        /// <summary>
        /// Create Mass object with the desired dimension.
        /// </summary>
        /// <param name="exponent">exponent of created Quantity</param>
        public Mass(float exponent) : base(exponent) { }


        private static readonly QuantityDimension _Dimension = new QuantityDimension(1, 0, 0);
        public override QuantityDimension Dimension => _Dimension * Exponent;


        public static implicit operator Mass<T>(T value)
        {
            var Q = new Mass<T>
            {
                Value = value
            };

            return Q;
        }

    }
}
