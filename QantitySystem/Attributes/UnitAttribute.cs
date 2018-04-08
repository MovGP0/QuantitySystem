using System;

namespace QuantitySystem.Attributes
{
    /*
     * Definitions:
     *      Quantity: The type of the value container.
     *      Value Container: The generic which hold the value.
     *      Units Cloud: set of units refer to the same Quantity by its Dimension in The Same system.
     *      System of Units: a set of different Quantities units grouped into known system {namespace} like imperial and SI or even egyptian 
     */
    /// <summary>
    /// The base unit attribute for all units.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UnitAttribute : Attribute
    {
        /// <summary>
        /// Unit Attribute Constructor.
        /// </summary>
        /// <param name="symbol">Symbol used for this unit.</param>
        /// <param name="quantityType">Quantity Type of this unit.</param>
        public UnitAttribute(string symbol, Type quantityType)
        {
            Symbol = symbol;
            QuantityType = quantityType;
        }

        public string Symbol { get; }

        public Type QuantityType { get; }
    }
}
