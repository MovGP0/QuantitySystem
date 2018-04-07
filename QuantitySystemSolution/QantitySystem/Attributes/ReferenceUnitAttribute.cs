using System;
using QuantitySystem.Exceptions;

namespace QuantitySystem.Attributes
{
    /// <summary>
    /// Make a relation between the attributed unit and a parent unit.
    /// If UnitType omitted the reference unit will be the default SI unit 
    /// of the QuantityType of the Unit or DefaultUnit Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ReferenceUnitAttribute : Attribute
    {
        private string Source { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numerator"></param>
        public ReferenceUnitAttribute(double numerator)
        {
            Numerator = numerator;
            Denominator = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        public ReferenceUnitAttribute(double numerator, double denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitType"></param>
        /// <param name="source">FunctionName.UnitName</param>
        public ReferenceUnitAttribute(Type unitType, string source)
        {
            Source = source;
            UnitType = unitType;
            
            Denominator = 1;

            // the numerator will be calculated based on source value

            var sourceFunctionName = source.Substring(0, source.IndexOf('.'));

            if (!DynamicQuantitySystem.DynamicSourceFunctions.ContainsKey(sourceFunctionName))
                DynamicQuantitySystem.DynamicSourceFunctions[sourceFunctionName] = u => 1.0;   // always return 1.0;
        }

        public Type UnitType { get; set; }

        /// <summary>
        /// Shift the conversion factor forward and backward.
        /// </summary>
        public double Shift { get; set; }

        private double _numerator;
        public double Numerator
        {
            get
            {
                if (!string.IsNullOrEmpty(Source))
                {
                    var sourceFunctionName = Source.Substring(0, Source.IndexOf('.'));
                    var unitKey = Source.Substring(Source.IndexOf('.') + 1);
                    return DynamicQuantitySystem.DynamicSourceFunctions[sourceFunctionName](unitKey);
                }
                
                return _numerator;
            }

            private set => _numerator = value;
        }

        public double Denominator { get; }
        public double Times => (Numerator / Denominator) + Shift;
    }
}
