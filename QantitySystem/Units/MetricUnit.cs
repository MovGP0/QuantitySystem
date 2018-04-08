using System;
using System.Linq;
using System.Reflection;
using QuantitySystem.Attributes;
using QuantitySystem.Exceptions;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units
{
    public abstract class MetricUnit : Unit
    {
        private static AnyQuantity<double> MakeQuantity(MetricUnit unit, MetricPrefix siPrefix, double value)
        {
            //assign its prefix
            unit.UnitPrefix = siPrefix;

            //create the corresponding quantity
            var qty = unit.GetThisUnitQuantity<double>();

            //assign the unit to the created quantity
            qty.Unit = unit;

            //assign the value to the quantity
            qty.Value = value;

            return qty;
        }

        public static AnyQuantity<double> Yotta<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Yotta, value);
        }

        public static AnyQuantity<double> Zetta<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Zetta, value);
        }

        public static AnyQuantity<double> Exa<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Exa, value);
        }

        public static AnyQuantity<double> Peta<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Peta, value);
        }

        public static AnyQuantity<double> Tera<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Tera, value);
        }

        public static AnyQuantity<double> Giga<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Giga, value);
        }

        public static AnyQuantity<double> Mega<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Mega, value);
        }

        public static AnyQuantity<double> Kilo<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Kilo, value);
        }

        public static AnyQuantity<double> Hecto<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Hecto, value);
        }

        public static AnyQuantity<double> Deka<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Deka, value);
        }

        public static AnyQuantity<double> None<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.None, value);
        }

        public static AnyQuantity<double> Deci<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Deci, value);
        }

        public static AnyQuantity<double> Centi<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Centi, value);
        }

        public static AnyQuantity<double> Milli<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Milli, value);
        }

        public static AnyQuantity<double> Micro<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Micro, value);
        }

        public static AnyQuantity<double> Nano<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Nano, value);
        }

        public static AnyQuantity<double> Pico<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Pico, value);
        }

        public static AnyQuantity<double> Femto<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Femto, value);
        }

        public static AnyQuantity<double> Atto<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Atto, value);
        }

        public static AnyQuantity<double> Zepto<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Zepto, value);
        }

        public static AnyQuantity<double> Yocto<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Yocto, value);
        }

        /// <summary>
        /// Return the SI Unit of strongly typed quantity.
        /// </summary>
        /// <typeparam name="TQuantity"></typeparam>
        /// <returns></returns>
        public static MetricUnit UnitOf<TQuantity>() where TQuantity : BaseQuantity, new()
        {
            //try direct mapping
            var unit = Activator.CreateInstance(GetDefaultSIUnitTypeOf(typeof(TQuantity))) as MetricUnit;

            if (unit != null)
            {
                return unit;
            }

            //if failed you should generate it
            //try first the child quantities in the quantity instance if its base is dervied quantity
            // and DerivedQuantity itself.

            var dimension = QuantityDimension.DimensionFrom(typeof(TQuantity));

            //return a derived unit.
            //return new DerivedSIUnit(dimension);
            throw new NotImplementedException();
        }

        public static AnyQuantity<double> GetUnitizedQuantityOf<TQuantity>(double value) where TQuantity : BaseQuantity, new()
        {
            var unit = UnitOf<TQuantity>();
            var aq = unit.GetThisUnitQuantity<double>();
            aq.Value = value;
            return aq;
        }

        protected MetricUnit()
        {
            //access the SIUnitAttribute
            MemberInfo info = GetType();

            var attributes = info.GetCustomAttributes(true);

            //get the UnitAttribute
            var siua = (MetricUnitAttribute)attributes.SingleOrDefault(ut => ut is MetricUnitAttribute);

            if (siua != null)
            {
                DefaultUnitPrefix = MetricPrefix.FromPrefixName(siua.SiPrefix.ToString());
                UnitPrefix = MetricPrefix.FromPrefixName(siua.SiPrefix.ToString());
            }
            else
            {
                throw new UnitException("SIUnitAttribute Not Found");
            }

            //ReferenceUnit attribute may or may not appear
            // if it appears then the unit is not a default SI unit
            // but act as SI Unit {means take prefixes}.  and accepted by the SI poids
            // however the code of reference unit is in the base class code.
        }

        /// <summary>
        /// Current unit default prefix.
        /// </summary>
        public MetricPrefix DefaultUnitPrefix { get; }

        /// <summary>
        /// Current instance unit prefix.
        /// </summary>
        public MetricPrefix UnitPrefix { get; set; }

        /// <summary>
        /// unit symbol with prefix.
        /// </summary>
        public override string Symbol => UnitPrefix.Symbol + base.Symbol;

        /// <summary>
        /// Tells if the current unit in default mode or not.
        /// </summary>
        public override bool IsDefaultUnit
        {
            get
            {
                //return true only and if only the current prefix equal the default prefix.
                if (_ReferenceUnit == null)
                {
                    //means I am native SI unit.
                    if (UnitPrefix.Exponent == DefaultUnitPrefix.Exponent)
                        return true;
                    return false;
                }

                //reference unit exist this is not native SI unit
                return false;
            }
        }

        public override Unit ReferenceUnit
        {
            get
            {
                if (_ReferenceUnit == null)
                {
                    // I am native SI unit
                    if (IsDefaultUnit)
                    {
                        //I am already default don't return extra parents.
                        return null;
                    }

                    // native si not in the default mode.
                    //put the default of this unit by creating it again
                    var refUnit = (MetricUnit)MemberwiseClone();
                    refUnit.UnitPrefix = DefaultUnitPrefix;
                    refUnit.UnitExponent = UnitExponent;

                    return refUnit;
                }

                //although it is inherited from SIUnit but the current instance
                // is most probably a unit accepted to be used in SIUnit
                // so it is not the default unit.
                // so we need the si reference unit.
                // the reference must be SI.
                return base.ReferenceUnit;
            }
        }

        public override double ReferenceUnitDenominator
        {
            get
            {
                if (_ReferenceUnit == null)
                {
                    if (IsDefaultUnit)
                        return 0;
                    return 1;
                }

                //return referenceUnitDenominator;
                return Math.Pow(_ReferenceUnitDenominator, UnitExponent);
            }
        }

        /// <summary>
        /// Reference unit is generated according to the state of the unit
        /// if the metric unit is not in the default prefix mode
        ///    the reference numerator is calculated based on difference between current prefix and default prefix
        ///    raised to the unit exponent.
        /// </summary>
        public override double ReferenceUnitNumerator
        {
            get
            {
                if (_ReferenceUnit == null)
                {
                    if (IsDefaultUnit)
                        return 0;
                    return Math.Pow(DefaultUnitPrefix.GetFactorForConvertTo(UnitPrefix), UnitExponent);
                }

                //convert me to default also if I had prefix over the default of me
                var correctToDefault = Math.Pow(DefaultUnitPrefix.GetFactorForConvertTo(UnitPrefix), UnitExponent);

                //p.u   where
                //      p: prefix
                //      u: metric unit
                //(p.u) i.e.  km, mm, Gare
                //(p.u)^r  = p^r*u^r

                return Math.Pow(_ReferenceUnitNumerator, UnitExponent) * correctToDefault;
            }
        }

        public override string ToString()
        {
            if (UnitPrefix.Exponent == 0)
                return GetType().Name + " " + Symbol;
            return UnitPrefix.Prefix + GetType().Name.ToLower() + " " + Symbol;
        }
    }
}
