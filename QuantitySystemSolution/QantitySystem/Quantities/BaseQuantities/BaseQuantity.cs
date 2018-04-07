using System;
using System.Collections.Generic;
using QuantitySystem.Exceptions;


namespace QuantitySystem.Quantities.BaseQuantities
{
    public abstract class BaseQuantity
    {
        

        #region Construction

        protected BaseQuantity(float exponent)
        {
            Exponent = exponent;
        }

        public float Exponent { get; private set; }

        internal void SetExponent(float exp)
        {
            Exponent = exp;
        }
        
        /// <summary>
        /// make 1/x operation.
        /// </summary>
        public virtual BaseQuantity Invert()
        {
            var bq = (BaseQuantity)MemberwiseClone();
            bq.SetExponent(0 - Exponent);
            

            return bq;
        }

        #endregion

        #region M L T  processing

        public virtual QuantityDimension Dimension => new QuantityDimension();

        #endregion




        #region Dimension Equality Algorithm

        /// <summary>
        /// Provides the Dimensional Equality algorithm.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var bd = obj as BaseQuantity;

            if(bd!=null)
            {

                if (Dimension.IsDimensionless & bd.Dimension.IsDimensionless)
                {
                    //why I've tested dimensioless in begining??
                    //   because I want special dimensionless quantities like angle and solid angle to be treated
                    //   as normal dimensionless values

                    return true;
                }

                return Dimension.Equals(bd.Dimension);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Dimension.GetHashCode();
        }

        #endregion



        /// <summary>
        /// Holds the internal quantity types of specific strongly typed quantity type.
        /// </summary>
        private static readonly Dictionary<Type, Tuple<Type, float>[]> QuantityTypeInternalQuantityTypes = new Dictionary<Type, Tuple<Type, float>[]>();

        public static Tuple<Type, float>[] GetInternalQuantities(Type quantity)
        {
            if (!QuantityTypeInternalQuantityTypes.TryGetValue(quantity, out var result))
            {
                // not instantiated yet .. so we can instantiate it here
                Activator.CreateInstance(quantity.MakeGenericType(typeof(double)));

                return QuantityTypeInternalQuantityTypes[quantity];
            }

            return result;
        }

        public static void SetInternalQuantities(Type quantity, Tuple<Type, float>[] internalQuantities)
        {
            if (!QuantityTypeInternalQuantityTypes.ContainsKey(quantity))
                QuantityTypeInternalQuantityTypes[quantity] = internalQuantities;
        }

    }


}
