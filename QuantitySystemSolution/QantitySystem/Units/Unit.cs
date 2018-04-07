using System;
using System.Collections.Generic;
using System.Linq;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;
using QuantitySystem.Exceptions;
using QuantitySystem.Quantities;
using QuantitySystem.Quantities.DimensionlessQuantities;
using QuantitySystem.Units.Metric.SI.BaseUnits;

namespace QuantitySystem.Units
{
    public class Unit
    {
        /// <summary>
        /// Creat units path from the current unit instance to the default unit of the current 
        /// unit system in the current quantity dimension.
        /// if the unit in the current system have no default unit and direct reference to SI
        /// then the path is stopped on the unit itself and shouldn't bypass it.
        /// </summary>
        /// <returns></returns>
        public UnitPathStack PathToDefaultUnit()
        {
            //from this unit get my path to the default unit.
            var path = new UnitPathStack();

            if (ReferenceUnit != null) //check that first parent exist.
            {
                var RefUnit = this;
                double RefTimesNum = 1;
                double RefTimesDen = 1;

                //double RefShift = 0.0;

                // do the iteration until we reach the default unit.
                while (RefUnit.IsDefaultUnit == false)
                {

                    path.Push(
                        new UnitPathItem
                        {
                            Unit = RefUnit,
                            Numerator = RefTimesNum,
                            Denominator = RefTimesDen,
                            //Shift = RefShift
                        }
                    );

                    RefTimesNum = RefUnit.ReferenceUnitNumerator;  //get the value before changing the RefUnit
                    RefTimesDen = RefUnit.ReferenceUnitDenominator;
                    //RefShift = RefUnit.ReferenceUnitShift;

                    RefUnit = RefUnit.ReferenceUnit;

                    // check the reference unit system or (namespace) if different throw exception.
                    //  the exception prevent crossing the system boundary.
                    //if (RefUnit.GetType().Namespace != this.GetType().Namespace) throw new UnitException("Unit system access violation");
                }

                // because of while there is another information should be put on the stack.
                path.Push(
                    new UnitPathItem
                    {
                        Unit = RefUnit,
                        Numerator = RefTimesNum,
                        Denominator = RefTimesDen,
                        //Shift = RefShift
                    }
                );
            }
            else
            {
                // no referenceUnit so this is SI unit because all my units ends with SI
                // and it is default unit because all si units have default units with the default prefix.
                if (QuantityType != typeof(DimensionlessQuantity<>))
                {
                    path.Push(
                        new UnitPathItem
                        {
                            Unit = this,
                            Numerator = 1,
                            Denominator = 1,
                            //Shift = 0.0
                        }
                    );
                }
            }

            return path;
        }


        /// <summary>
        /// Create units path from default unit in the dimension of the current unit system to the running unit instance.
        /// </summary>
        /// <returns></returns>
        public UnitPathStack PathFromDefaultUnit()
        {
            var Forward = PathToDefaultUnit();

            var Backward = new UnitPathStack();

            while (Forward.Count > 0)
            {
                var upi = Forward.Pop();

                if (upi.Unit.IsDefaultUnit)
                {
                    upi.Numerator = 1;
                    upi.Denominator = 1;
                    //upi.Shift = 0;
                }
                else
                {
                    upi.Numerator = upi.Unit.ReferenceUnitDenominator;  //invert the number
                    upi.Denominator = upi.Unit.ReferenceUnitNumerator;
                    //upi.Shift = 0 - upi.Unit.ReferenceUnitShift;
                }

                Backward.Push(upi);
            }

            return Backward;
        }


        /// <summary>
        /// Create units path from unit to unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public UnitPathStack PathFromUnit(Unit unit)
        {

            return unit.PathToUnit(this);

        }

        public Func<Unit, Unit, string> UnitToUnitSymbol = (x, y) => "[" + x.Symbol + ":" + x.UnitDimension.ToString() + "]" + "__" + "[" + y.Symbol + ":" + y.UnitDimension.ToString() + "]";

        private static readonly Dictionary<string, UnitPathStack> CachedPaths = new Dictionary<string, UnitPathStack>();

        private static bool enableUnitsCaching = true;
        public static bool EnableUnitsCaching
        {
            get => enableUnitsCaching;
            set
            {
                enableUnitsCaching = value;
                if (enableUnitsCaching) CachedPaths.Clear();
            }
        }

        /// <summary>
        /// Gets the path to the unit starting from current unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public UnitPathStack PathToUnit(Unit unit)
        {
            lock (CachedPaths)
            {
                #region Caching
                //because this method can be a lengthy method we try to check for cached pathes first.
                if (EnableUnitsCaching)
                {
                    if (CachedPaths.TryGetValue(UnitToUnitSymbol(this, unit), out var cachedPath))
                    {
                        return cachedPath.Clone();   //<--- Clone

                        //Why CLONE :D ??  because the unit path is a stack and I use Pop all the time 
                        // during the application, and there were hidden error that poping from unit path in the 
                        // cached store will not get them back again ;)
                        //  I MUST return cloned copy of the UnitPath.

                    }
                }
                #endregion

                #region validity of conversion
                if (UnitDimension.IsDimensionless && unit.UnitDimension.IsDimensionless)
                {

                }
                else
                {
                    //why I've tested dimensioless in begining??
                    //   because I want special dimensionless quantities like angle and solid angle to be treated
                    //   as normal dimensionless values

                    if (UnitDimension.Equals(unit.UnitDimension) == false)
                    {
                        throw new UnitsNotDimensionallyEqualException();
                    }
                }
                #endregion

                //test if one of the units are not strongly typed
                //  because this needs special treatment. ;)
                if (IsStronglyTyped == false || unit.IsStronglyTyped == false)
                {
                    #region Complex units

                    //the unit is not strongly typed so we need to make conversion to get its conversion
                    // Source unit ==> SI Base Units
                    // target unit ==> SI BaseUnits

                    var SourcePath = PathToSIBaseUnits();
                    var TargetPath = unit.PathToSIBaseUnits();

                    var Tito = new UnitPathStack();

                    while (SourcePath.Count > 0)
                    {
                        Tito.Push(SourcePath.Pop());
                    }
                    //we have to invert the target 
                    while (TargetPath.Count > 0)
                    {
                        var upi = TargetPath.Pop();
                        upi.Invert();

                        Tito.Push(upi);

                    }

                    //first location in cache look below for the second location.

                    if (EnableUnitsCaching)
                    {
                        CachedPaths.Add(UnitToUnitSymbol(this, unit), Tito.Clone());
                    }

                    return Tito;

                    #endregion
                }

                // 1- Get Path default unit to current unit.

                var fromMeToDefaultUnit = PathToDefaultUnit();

                // 2- Get Path From Default unit to the passed unit.

                var fromDefaultUnitToTargetUnit = unit.PathFromDefaultUnit();

                // 3- check if the two units are in the same unit system
                //  if the units share the same parent don't jump

                UnitPathStack systemsPath = null;

                var noBoundaryCross = false;

                if (UnitSystem == unit.UnitSystem)
                {
                    noBoundaryCross = true;
                }
                else
                {
                    //test for that units parents are the same

                    var thisParent = UnitSystem.IndexOf('.') > -1 ?
                        UnitSystem.Substring(0, UnitSystem.IndexOf('.')) :
                        UnitSystem;

                    var targetParent = unit.UnitSystem.IndexOf('.') > -1 ?
                        unit.UnitSystem.Substring(0, unit.UnitSystem.IndexOf('.')) :
                        unit.UnitSystem;

                    if (thisParent == targetParent) noBoundaryCross = true;
                }

                if (noBoundaryCross)
                {
                    //no boundary cross should occur

                    //if the two units starts with Metric then no need to cross boundaries because
                    //they have common references in metric.
                }
                else
                {
                    //then we must go out side the current unit system
                    //all default units are pointing to the SIUnit system this is a must and not option.

                    //get the default unit of target 


                    // to cross the boundary
                    // we should know the non SI system that we will cross it
                    // we have two options

                    // 1- FromMeToDefaultUnit if (Me unit is another system (not SI)
                    //     in this case we will take the top unit to get its reference
                    // 2- FromDefaultUnitToTargetUnit (where default unit is not SI)
                    //     and in this case we will take the last bottom unit of stack and get its reference
                    systemsPath = new UnitPathStack();

                    UnitPathItem DefaultPItem;
                    UnitPathItem RefUPI;

                    var SourceDefaultUnit = fromMeToDefaultUnit.Peek().Unit;

                    if (SourceDefaultUnit.UnitSystem != "Metric.SI" && SourceDefaultUnit.GetType() != typeof(Shared.Second))
                    {
                        //from source default unit to the si
                        DefaultPItem = fromMeToDefaultUnit.Peek();
                        RefUPI = new UnitPathItem
                        {
                            Numerator = DefaultPItem.Unit.ReferenceUnitNumerator,
                            Denominator = DefaultPItem.Unit.ReferenceUnitDenominator,
                            //Shift = DefaultPItem.Unit.ReferenceUnitShift,
                            Unit = DefaultPItem.Unit.ReferenceUnit
                        };
                    }
                    else
                    {
                        // from target default unit to si
                        DefaultPItem = fromDefaultUnitToTargetUnit.ElementAt(fromDefaultUnitToTargetUnit.Count - 1);
                        RefUPI = new UnitPathItem
                        {
                            //note the difference here 
                            //I made the opposite assignments because we are in reverse manner

                            Numerator = DefaultPItem.Unit.ReferenceUnitDenominator, // <=== opposite
                            Denominator = DefaultPItem.Unit.ReferenceUnitNumerator, // <===
                            //Shift = 0-DefaultPItem.Unit.ReferenceUnitShift,
                            Unit = DefaultPItem.Unit.ReferenceUnit
                        };
                    }

                    if (RefUPI.Unit != null)
                    {
                        systemsPath.Push(RefUPI);
                    }
                }

                //combine the two paths
                var Total = new UnitPathStack();

                //we are building the conversion stairs
                // will end like a stack


                //begin from me unit to default unit
                for (var i = fromMeToDefaultUnit.Count - 1; i >= 0; i--)
                {
                    Total.Push(fromMeToDefaultUnit.ElementAt(i));
                }

                var One = new Unit(typeof(DimensionlessQuantity<>));

                //cross the system if we need to .
                if (systemsPath != null)
                {
                    Total.Push(new UnitPathItem { Denominator = 1, Numerator = 1, Unit = One });
                    for (var i = systemsPath.Count - 1; i >= 0; i--)
                    {
                        Total.Push(systemsPath.ElementAt(i));
                    }
                    Total.Push(new UnitPathItem { Denominator = 1, Numerator = 1, Unit = One });
                }

                // from default unit to target unit
                for (var i = fromDefaultUnitToTargetUnit.Count - 1; i >= 0; i--)
                {
                    Total.Push(fromDefaultUnitToTargetUnit.ElementAt(i));
                }

                //another check if the units are inverted then 
                // go through all items in path and invert it also

                if (IsInverted && unit.IsInverted)
                {
                    foreach (var upi in Total)
                        upi.Invert();
                }

                //Second location in cache  look above for the first one in the same function here :D
                if (EnableUnitsCaching)
                {
                    CachedPaths.Add(UnitToUnitSymbol(this, unit), Total.Clone());
                }

                return Total;
            }
        }

        public UnitPathStack PathToSIBaseUnits()
        {
            UnitPathStack up;
            if (IsStronglyTyped)
            {
                //get the corresponding unit in the SI System
                var innerUnitType = GetDefaultSIUnitTypeOf(QuantityType);

                if (innerUnitType == null && QuantityType == typeof(PolarLength<>))
                    innerUnitType = GetDefaultSIUnitTypeOf(typeof(Length<>));

                if (innerUnitType == null)
                {
                    //some quantities don't have strongly typed si units

                    //like knot unit there are no corresponding velocity unit in SI
                    //  we need to replace the knot unit with mixed unit to be able to do the conversion

                    // first we should reach default unit
                    var path = PathToDefaultUnit();

                    //then test the system of the current unit if it was other than Metric.SI
                    //    then we must jump to SI otherwise we are already in default SI
                    if (UnitSystem == "Metric.SI" && UnitExponent == 1)
                    {

                        //because no unit in SI with exponent = 1 don't have direct unit type
                        throw new NotImplementedException("Impossible reach by logic");
                    }

                    // We should cross the system boundary 

                    var defaultUnitPathItem = path.Peek();
                    if (defaultUnitPathItem.Unit.ReferenceUnit != null)
                    {
                        var referenceUnitPathItem = new UnitPathItem
                        {
                            Numerator = defaultUnitPathItem.Unit.ReferenceUnitNumerator,
                            Denominator = defaultUnitPathItem.Unit.ReferenceUnitDenominator,
                            Unit = defaultUnitPathItem.Unit.ReferenceUnit
                        };

                        path.Push(referenceUnitPathItem);
                    }
                    return path;
                }

                var SIUnit = (Unit)Activator.CreateInstance(innerUnitType);
                SIUnit.UnitExponent = UnitExponent;
                SIUnit.UnitDimension = UnitDimension;

                up = PathToUnit(SIUnit);

                if (!SIUnit.IsBaseUnit)
                {
                    if (SIUnit.UnitDimension.IsDimensionless && SIUnit.IsStronglyTyped)
                    {
                        //for dimensionless units like radian, stradian
                        //do nothing.
                    }
                    else
                    {
                        //expand the unit 
                        var expandedUnit = ExpandMetricUnit((MetricUnit)SIUnit);
                        var expath = expandedUnit.PathToSIBaseUnits();

                        while (expath.Count > 0)
                            up.Push(expath.Pop());
                    }
                }

                return up;
            }

            var pathes = new UnitPathStack();
            foreach (var un in SubUnits)
            {
                up = un.PathToSIBaseUnits();

                while (up.Count > 0)
                {
                    var upi = up.Pop();

                    if (un.IsInverted) upi.Invert();

                    pathes.Push(upi);
                }
            }
            return pathes;
        }

        /// <summary>
        /// Returns quantity based on current unit instance.
        /// </summary>
        /// <typeparam name="T">Quatntity Storage Type</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        internal AnyQuantity<T> MakeQuantity<T>(T value)
        {
            //create the corresponding quantity
            var qty = GetThisUnitQuantity<T>();

            //assign the unit to the created quantity
            qty.Unit = this;

            //assign the value to the quantity
            qty.Value = value;

            return qty;
        }

        /// <summary>
        /// Returns quantity from the current unit type.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static AnyQuantity<double> QuantityOf<TUnit>(double value)
            where TUnit : Unit, new()
        {
            Unit unit = new TUnit();
            return unit.MakeQuantity(value);
        }

        #region Helper Properties
        private static Type[] unitTypes;

        /// <summary>
        /// All Types inherited from Unit Type.
        /// </summary>
        public static Type[] UnitTypes
        {
            get
            {
                if (unitTypes == null)
                {
                    var AllTypes = Assembly.GetExecutingAssembly().GetTypes();

                    var Types = from si in AllTypes
                                where si.IsSubclassOf(typeof(Unit))
                                select si;

                    unitTypes = Types.ToArray();

                }
                return unitTypes;
            }
        }
        #endregion

        #region Helper Functions and Properties




        /// <summary>
        /// Gets the default unit type of quantity type parameter based on the unit system (namespace)
        /// under the Units name space.
        /// The Default Unit Type for length in Imperial is Foot for example.
        /// </summary>
        /// <param name="quantityType">quantity type</param>
        /// <param name="unitSystem">The Unit System or explicitly the namespace under Units Namespace</param>
        /// <returns>Unit Type Based on the unit system</returns>
        public static Type GetDefaultUnitTypeOf(Type quantityType, string unitSystem)
        {
            unitSystem = unitSystem.ToUpperInvariant();

            if (unitSystem.Contains("METRIC.SI"))
            {
                var oUnitType = GetDefaultSIUnitTypeOf(quantityType);
                return oUnitType;
            }

            //getting the generic type
            if (!quantityType.IsGenericTypeDefinition)
            {
                //the passed type is AnyQuantity<object> for example
                //I want to get the type without type parameters AnyQuantity<>

                quantityType = quantityType.GetGenericTypeDefinition();

            }


            //predictor of default unit.
            Func<Type, bool> SearchForQuantityType = unitType =>
            {
                //search in the attributes of the unit type
                var info = unitType as MemberInfo;

                var attributes = info.GetCustomAttributes(true);

                //get the UnitAttribute
                var ua = (UnitAttribute)attributes.SingleOrDefault(ut => ut is UnitAttribute);

                if (ua != null)
                {
                    if (ua.QuantityType == quantityType)
                    {
                        if (ua is DefaultUnitAttribute)
                        {
                            //explicitly default unit.
                            return true;
                        }

                        if (ua is MetricUnitAttribute)
                        {
                            //check if the unit has SystemDefault flag true or not.
                            var mua = ua as MetricUnitAttribute;
                            if (mua.SystemDefault)
                            {
                                return true;
                            }

                            return false;
                        }

                        return false;
                    }

                    return false;

                }

                return false;
            };


            var CurrentUnitSystem = unitSystem; //
            Type SystemUnitType = null;

            //search in upper namespaces also to get the default unit of the parent system.
            while (string.IsNullOrEmpty(CurrentUnitSystem) == false && SystemUnitType == null)
            {

                //prepare the query that we will search in
                var SystemUnitTypes = from ut in UnitTypes
                                      where ut.Namespace.ToUpperInvariant().EndsWith(CurrentUnitSystem, StringComparison.Ordinal)
                                            || ut.Namespace.ToUpperInvariant().EndsWith("SHARED", StringComparison.Ordinal)
                                      select ut;

                //select the default by predictor from the query
                SystemUnitType = Enumerable.SingleOrDefault(SystemUnitTypes, SearchForQuantityType
                );

                if (CurrentUnitSystem.LastIndexOf('.') < 0)
                {
                    CurrentUnitSystem = "";
                }
                else
                {
                    CurrentUnitSystem = CurrentUnitSystem.Substring(0, CurrentUnitSystem.LastIndexOf('.'));
                }

            }

            if (SystemUnitType == null && unitSystem.Contains("METRIC"))
            {
                //try another catch for SI unit for this quantity
                //   because SI and metric units are disordered for now
                // so if the search of unit in parent metric doesn't exist then search for it in SI units.

                SystemUnitType = GetDefaultSIUnitTypeOf(quantityType);
            }

            return SystemUnitType;
        }


        /// <summary>
        /// Gets the unit type of quantity type parameter based on SI unit system.
        /// The function is direct mapping from types of quantities to types of units.
        /// if function returns null then this quantity dosen't have a statically linked unit to it.
        /// this means the quantity should return a unit in runtime.
        /// </summary>
        /// <param name="qType">Type of Quantity</param>
        /// <returns>SI Unit Type</returns>
        public static Type GetDefaultSIUnitTypeOf(Type qType)
        {

            var quantityType = qType;


            //getting the generic type
            if (!quantityType.IsGenericTypeDefinition)
            {
                //the passed type is AnyQuantity<object> for example
                //I want to get the type without type parameters AnyQuantity<>

                quantityType = quantityType.GetGenericTypeDefinition();

            }

            //don't forget to include second in si units it is shared between all metric systems
            var SIUnitTypes = from si in UnitTypes
                              where si.Namespace.ToUpperInvariant().EndsWith("SI", StringComparison.Ordinal) || si.Namespace.ToUpperInvariant().EndsWith("SHARED", StringComparison.Ordinal)
                              select si;



            Func<Type, bool> SearchForQuantityType = unitType =>
            {
                //search in the attributes of the unit type
                var info = unitType as MemberInfo;

                var attributes = info.GetCustomAttributes(true);

                //get the UnitAttribute
                var ua = (UnitAttribute)attributes.SingleOrDefault(ut => ut is UnitAttribute);

                if (ua != null)
                {
                    if (ua.QuantityType == quantityType)
                        return true;
                    return false;

                }

                return false;
            };


            var SIUnitType = Enumerable.SingleOrDefault(SIUnitTypes, SearchForQuantityType
            );

            return SIUnitType;
        }

        #endregion

        /// <summary>
        /// Find Strongly typed unit.
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        private static Unit FindUnit(string unitName)
        {
            var unit = unitName.Replace("$", "\\$");

            var UnitModifier = false;

            if (unit.EndsWith("!", StringComparison.Ordinal))
            {
                //it is intended of Radius length
                unit = unit.TrimEnd('!');
                UnitModifier = true; //unit modifier have one use for now is to convert the Length Quantity into Length Quantity into RadiusLength quantity
            }

            foreach (var unitType in UnitTypes)
            {
                var ua = GetUnitAttribute(unitType);
                if (ua != null)
                {
                    //units are case sensitive
                    if (Regex.Match(ua.Symbol, "^" + unit + "$", RegexOptions.Singleline).Success)
                    {
                        var u = (Unit)Activator.CreateInstance(unitType);

                        //test unit if it is metric so that we remove the default prefix that created with it
                        if (u is MetricUnit metricUnit)
                        {
                            metricUnit.UnitPrefix = MetricPrefix.None;
                        }

                        if (UnitModifier)
                        {
                            //test if the dimension is length and modify it to be radius length
                            if (u.QuantityType == typeof(Length<>))
                            {
                                u.QuantityType = typeof(PolarLength<>);
                            }
                        }

                        return u;
                    }
                }
            }

            throw new UnitNotFoundException(unitName);
        }

        /// <summary>
        /// Parse units with exponent and one division '/' with many '.'
        /// i.e. m/s m/s^2 kg.m/s^2
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        public static Unit Parse(string units)
        {
            //  if found  treat store its value.
            //  m/s^2   m.K/m.s
            //  kg^2/in^2.s

            // sea
            //search for '/'

            var uny = units.Split('/');

            var numa = uny[0].Split('.');

            var dunits = new List<Unit>();
            foreach (var num in numa)
            {
                dunits.Add(ParseUnit(num));
            }

            if (uny.Length > 1)
            {
                var dena = uny[1].Split('.');
                foreach (var den in dena)
                {
                    var uu = ParseUnit(den);
                    //then it is unit with sub units in it
                    if (uu.SubUnits?.Count == 1) uu = uu.SubUnits[0];
                    dunits.Add(uu.Invert());
                }
            }

            if (dunits.Count == 1) return dunits[0];

            //get the dimension of all units
            var ud = QuantityDimension.Dimensionless;

            foreach (var un in dunits)
            {
                ud += un.UnitDimension;
            }

            var uQType = QuantityDimension.GetQuantityTypeFrom(ud);

            return new Unit(uQType, dunits.ToArray());
        }

        /// <summary>
        /// Returns the unit corresponding to the passed string.
        /// Suppors units with exponent.
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        internal static Unit ParseUnit(string unitName)
        {
            if (unitName == "1")
            {
                //this is dimensionless value
                return new Unit(typeof(DimensionlessQuantity<>));
            }

            //find '^'

            var upower = unitName.Split('^');
            var unit = upower[0];
            var power = 1;

            if (upower.Length > 1) power = int.Parse(upower[1], CultureInfo.InvariantCulture);

            Unit finalUnit = null;

            //Phase 1: try direct mapping.
            try
            {
                finalUnit = FindUnit(unit);
            }
            catch (UnitNotFoundException)
            {
                //try to find if it as a Metric unit with prefix
                //loop through all prefixes.
                for (var i = 10; i >= -10; i -= 1)
                {
                    if (i == 0) i--; //skip the None prefix
                    if (unit.StartsWith(MetricPrefix.GetPrefix(i).Symbol, StringComparison.Ordinal))
                    {
                        //found

                        var mp = MetricPrefix.GetPrefix(i);
                        var upart = unit.Substring(mp.Symbol.Length);

                        //then it should be MetricUnit otherwise die :)

                        var u = FindUnit(upart) as MetricUnit;

                        if (u == null) goto nounit;

                        u.UnitPrefix = mp;

                        finalUnit = u;
                        break;
                    }
                }
            }

            if (finalUnit == null) goto nounit;

            if (power > 1)
            {
                //discover the new type
                var ud = finalUnit.UnitDimension * power;

                var chobits = new Unit[power];  //what is chobits any way :O

                for (var iy = 0; iy < power; iy++)
                    chobits[iy] = (Unit)finalUnit.MemberwiseClone();

                var uQType = QuantityDimension.GetQuantityTypeFrom(ud);

                finalUnit = new Unit(uQType, chobits);
            }

            return finalUnit;

            nounit:
            throw new UnitNotFoundException(unitName);
        }

        private static readonly Dictionary<Type, UnitAttribute> UnitsAttributes = new Dictionary<Type, UnitAttribute>();

        /// <summary>
        /// Get the unit attribute which hold the unit information.
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public static UnitAttribute GetUnitAttribute(Type unitType)
        {
            if (!UnitsAttributes.TryGetValue(unitType, out var ua))
            {
                var attributes = unitType.GetCustomAttributes(typeof(UnitAttribute), true);

                //get the UnitAttribute
                ua = (UnitAttribute)attributes.SingleOrDefault(ut => ut is UnitAttribute);
                UnitsAttributes.Add(unitType, ua);
            }

            return ua;
        }

        /// <summary>
        /// Returns the unit of strongly typed metric unit to unit with sub units as base units
        /// and add the prefix to the expanded base units.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static Unit ExpandMetricUnit(MetricUnit unit)
        {
            var defaultUnits = new List<Unit>();

            if (unit.IsBaseUnit)
            {
                //if baseunit then why we would convert it
                // put it immediately
                return unit;
            }

            var qdim = QuantityDimension.DimensionFrom(unit.QuantityType);

            if (unit is MetricUnit)
            {
                //pure unit without sub units like Pa, N, and L

                var u = DiscoverUnit(qdim);

                var baseUnits = u.SubUnits;

                //add prefix to the first unit in the array

                ((MetricUnit)baseUnits[0]).UnitPrefix += unit.UnitPrefix;

                defaultUnits.AddRange(baseUnits);
            }

            return new Unit(unit._QuantityType, defaultUnits.ToArray());
        }

        /// <summary>
        /// Takes string of the form number and unit i.e. "50.34 &lt;kg>"
        /// and returns Quantity of the discovered unit.
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static AnyQuantity<double> ParseQuantity(string quantity)
        {
            if (TryParseQuantity(quantity, out var aty))
                return aty;
            throw new QuantityException("Couldn't parse to quantity");
        }

        private const string DoubleNumber = @"[-+]?\d+(\.\d+)*([eE][-+]?\d+)*";

        private const string UnitizedNumber = "^(?<num>" + DoubleNumber + @")\s*<(?<unit>([\w\$\.\^/!]+)?)>$";
        private static readonly Regex UnitizedNumberRegex = new Regex(UnitizedNumber);

        public static bool TryParseQuantity(string quantity, out AnyQuantity<double> qty)
        {
            double val;

            var um = UnitizedNumberRegex.Match(quantity.Trim());
            if (um.Success)
            {
                var varUnit = um.Groups["unit"].Value;
                val = double.Parse(um.Groups["num"].Value, CultureInfo.InvariantCulture);

                var un = Parse(varUnit);
                qty = un.GetThisUnitQuantity(val);

                return true;
            }

            if (double.TryParse(quantity, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
            {
                qty = DiscoverUnit(QuantityDimension.Dimensionless).GetThisUnitQuantity(val);

                return true;
            }

            qty = default(AnyQuantity<double>);
            return false;
        }

        public Unit RaiseUnitPower(float power)
        {
            //make short-cut when power equal zero return dimensionless unit immediatly
            //  because if I left the execution to the end 
            //  the dimensionless unit is created and wrapping the original unit under sub unit
            //    and this made errors in conversion between dimensionless units :).
            if (power == 0)
            {
                return DiscoverUnit(QuantityDimension.Dimensionless);
            }

            var u = (Unit)MemberwiseClone();

            if (SubUnits != null)
            {
                u.SubUnits = new List<Unit>(SubUnits.Count);

                for (var i = 0; i < SubUnits.Count; i++)
                {
                    u.SubUnits.Add(SubUnits[i].RaiseUnitPower(power));
                }
            }
            else
            {
                u.UnitExponent *= power;   //the exponent is changing in strongly typed units
            }

            u._UnitDimension = _UnitDimension * power; //must change the unit dimension of the unit
            // however because the unit is having sub units we don't have to modify the exponent of it
            //  note: unit that depend on sub units is completly unaware of its exponent
            //    or I should say it is always equal = 1

            u._QuantityType = QuantityDimension.GetQuantityTypeFrom(u._UnitDimension);

            if (u.SubUnits == null && u.UnitExponent == 1)
            {
                //no sub units and exponent ==1  then no need to processing
                return u;
            }

            if (u.SubUnits == null)
            {
                //exponent != 1  like ^5 ^0.3  we need processing
                return new Unit(u._QuantityType, u);
            }

            //consist of sub units definitly we need processing.
            return new Unit(u._QuantityType, u.SubUnits.ToArray());
        }

        #region Dynamically created unit

        private List<Unit> SubUnits { get; set; } //the list shouldn't been modified by sub classes

        /// <summary>
        /// Create the unit directly from the specfied dimension in its SI base units.
        /// </summary>
        /// <param name="dimension"></param>
        public static Unit DiscoverUnit(QuantityDimension dimension)
        {
            return DiscoverUnit(dimension, "Metric.SI");
        }

        /// <summary>
        /// Create the unit directly from the specfied dimension based on the unit system given.
        /// </summary>
        /// <param name="dimension"></param>
        public static Unit DiscoverUnit(QuantityDimension dimension, string unitSystem)
        {
            var subUnits = new List<Unit>();

            if (dimension.Currency.Exponent != 0)
            {
                Unit u = new Currency.Coin
                {
                    UnitExponent = dimension.Currency.Exponent,
                    UnitDimension = new QuantityDimension { Currency = new DimensionDescriptors.CurrencyDescriptor(dimension.Currency.Exponent) }
                };
                subUnits.Add(u);

            }

            if (dimension.Mass.Exponent != 0)
            {
                var unitType = GetDefaultUnitTypeOf(typeof(Mass<>), unitSystem);

                var u = (Unit)Activator.CreateInstance(unitType);

                u.UnitExponent = dimension.Mass.Exponent;
                u.UnitDimension = new QuantityDimension(dimension.Mass.Exponent, 0, 0);

                subUnits.Add(u);
            }

            if (dimension.Length.Exponent != 0)
            {
                var unitType = GetDefaultUnitTypeOf(typeof(Length<>), unitSystem);

                var u = (Unit)Activator.CreateInstance(unitType);

                u.UnitExponent = dimension.Length.Exponent;
                u.UnitDimension = new QuantityDimension { Length = dimension.Length };

                subUnits.Add(u);
            }

            if (dimension.Time.Exponent != 0)
            {
                var unitType = GetDefaultUnitTypeOf(typeof(Time<>), unitSystem);

                var u = (Unit)Activator.CreateInstance(unitType);

                u.UnitExponent = dimension.Time.Exponent;
                u.UnitDimension = new QuantityDimension { Time = dimension.Time };

                subUnits.Add(u);
            }

            if (dimension.Temperature.Exponent != 0)
            {
                var unitType = GetDefaultUnitTypeOf(typeof(Temperature<>), unitSystem);

                var u = (Unit)Activator.CreateInstance(unitType);

                u.UnitExponent = dimension.Temperature.Exponent;
                u.UnitDimension = new QuantityDimension { Temperature = dimension.Temperature };

                subUnits.Add(u);
            }

            if (dimension.LuminousIntensity.Exponent != 0)
            {
                var unitType = GetDefaultUnitTypeOf(typeof(LuminousIntensity<>), unitSystem);

                var u = (Unit)Activator.CreateInstance(unitType);

                u.UnitExponent = dimension.LuminousIntensity.Exponent;
                u.UnitDimension = new QuantityDimension { LuminousIntensity = dimension.LuminousIntensity };

                subUnits.Add(u);
            }

            if (dimension.AmountOfSubstance.Exponent != 0)
            {
                var unitType = GetDefaultUnitTypeOf(typeof(AmountOfSubstance<>), unitSystem);

                var u = (Unit)Activator.CreateInstance(unitType);

                u.UnitExponent = dimension.AmountOfSubstance.Exponent;
                u.UnitDimension = new QuantityDimension(0, 0, 0, 0, 0, dimension.AmountOfSubstance.Exponent, 0);

                subUnits.Add(u);
            }

            if (dimension.ElectricCurrent.Exponent != 0)
            {
                var unitType = GetDefaultUnitTypeOf(typeof(ElectricalCurrent<>), unitSystem);

                var u = (Unit)Activator.CreateInstance(unitType);

                u.UnitExponent = dimension.ElectricCurrent.Exponent;
                u.UnitDimension = new QuantityDimension { ElectricCurrent = dimension.ElectricCurrent };
                subUnits.Add(u);
            }

            if (dimension.Currency.Exponent != 0)
            {
                Unit u = new Currency.Coin
                {
                    UnitExponent = dimension.Currency.Exponent
                };
                u.UnitDimension = u.UnitDimension * dimension.Currency.Exponent;

                subUnits.Add(u);
            }

            if (dimension.Digital.Exponent != 0)
            {
                Unit u = new Digital.Bit
                {
                    UnitExponent = dimension.Digital.Exponent
                };
                u.UnitDimension = u.UnitDimension * dimension.Digital.Exponent;

                subUnits.Add(u);
            }

            try
            {
                var qType = QuantityDimension.QuantityTypeFrom(dimension);
                return new Unit(qType, subUnits.ToArray());
            }
            catch (QuantityNotFoundException)
            {
                return new Unit(null, subUnits.ToArray());
            }
        }

        /// <summary>
        /// Construct a unit based on the quantity type in SI Base units.
        /// Any Dimensionless quantity will return  in its unit.
        /// </summary>
        /// <param name="quantityType"></param>
        public Unit(Type quantityType)
        {
            SubUnits = new List<Unit>();

            //try direct mapping first to get the unit

            var innerUnitType = GetDefaultSIUnitTypeOf(quantityType);

            if (innerUnitType == null)
            {
                var dimension = QuantityDimension.DimensionFrom(quantityType);

                if (dimension.Mass.Exponent != 0)
                {
                    Unit u = new Gram
                    {
                        UnitExponent = dimension.Mass.Exponent
                    };
                    u.UnitDimension = u.UnitDimension * dimension.Mass.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.Length.Exponent != 0)
                {
                    Unit u = new Metre
                    {
                        UnitExponent = dimension.Length.Exponent
                    };
                    u.UnitDimension = u.UnitDimension * dimension.Length.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.Time.Exponent != 0)
                {
                    Unit u = new Shared.Second
                    {
                        UnitExponent = dimension.Time.Exponent
                    };
                    u.UnitDimension = u.UnitDimension * dimension.Time.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.Temperature.Exponent != 0)
                {
                    Unit u = new Kelvin
                    {
                        UnitExponent = dimension.Temperature.Exponent
                    };
                    u.UnitDimension = u.UnitDimension * dimension.Temperature.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.LuminousIntensity.Exponent != 0)
                {
                    Unit u = new Candela
                    {
                        UnitExponent = dimension.LuminousIntensity.Exponent
                    };
                    u.UnitDimension = u.UnitDimension * dimension.LuminousIntensity.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.AmountOfSubstance.Exponent != 0)
                {
                    Unit u = new Mole
                    {
                        UnitExponent = dimension.AmountOfSubstance.Exponent
                    };
                    u.UnitDimension = u.UnitDimension * dimension.AmountOfSubstance.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.ElectricCurrent.Exponent != 0)
                {
                    Unit u = new Ampere
                    {
                        UnitExponent = dimension.ElectricCurrent.Exponent
                    };
                    u.UnitDimension = u.UnitDimension * dimension.ElectricCurrent.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.Currency.Exponent != 0)
                {
                    Unit u = new Currency.Coin
                    {
                        UnitExponent = dimension.Currency.Exponent
                    };
                    u.UnitDimension = u.UnitDimension * dimension.Currency.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.Digital.Exponent != 0)
                {
                    Unit u = new Digital.Bit
                    {
                        UnitExponent = dimension.Digital.Exponent
                    };
                    u.UnitDimension = u.UnitDimension * dimension.Digital.Exponent;

                    SubUnits.Add(u);
                }
            }
            else
            {
                //subclass of AnyQuantity
                //use direct mapping with the exponent of the quantity

                var un = (Unit)Activator.CreateInstance(innerUnitType);

                SubUnits.Add(un);
            }

            _Symbol = GenerateUnitSymbolFromSubBaseUnits();


            _IsDefaultUnit = true;

            //the quantity may be derived quantity which shouldn't be referenced :check here.
            _QuantityType = quantityType;

            _UnitDimension = QuantityDimension.DimensionFrom(_QuantityType);

            IsBaseUnit = false;
        }

        /// <summary>
        /// Construct a unit based on the default units of the internal quantities of passed quantity instance.
        /// Dimensionless quantity will return their native sub quantities units.
        /// this connstructor is useful like when you pass torque quantity it will return "N.m"
        /// but when you use Energy Quantity it will return J.
        /// </summary>
        /// <param name="quantity"></param>
        public static Unit DiscoverUnit(BaseQuantity quantity)
        {
            var m_QuantityType = quantity.GetType();

            var gen_q = m_QuantityType.GetGenericTypeDefinition();

            if (gen_q == typeof(Currency<>)) return new Currency.Coin();
            if (gen_q == typeof(Digital<>)) return new Digital.Bit();

            if (gen_q == typeof(PolarLength<>))
            {
                //because all length units associated with the Length<> Type
                m_QuantityType = typeof(Length<>).MakeGenericType(m_QuantityType.GetGenericArguments()[0]);
            }

            if (quantity.Dimension.IsDimensionless)
            {
                var QtyType = m_QuantityType;
                if (!QtyType.IsGenericTypeDefinition)
                {
                    QtyType = QtyType.GetGenericTypeDefinition();

                }

                if (QtyType == typeof(DimensionlessQuantity<>))
                {
                    return DiscoverUnit(QuantityDimension.Dimensionless);
                }
            }

            var subUnits = new List<Unit>();

            //try direct mapping first to get the unit

            var innerUnitType = GetDefaultSIUnitTypeOf(m_QuantityType);

            if (innerUnitType == null) //no direct mapping so get it from the inner quantities
            {
                //I can't cast BaseQuantity to AnyQuantity<object>  very annoying
                //so I used reflection.

                var giq = m_QuantityType.GetMethod("GetInternalQuantities");

                //casted the array to BaseQuantity array also
                var internalQuantities = giq.Invoke(quantity, null) as BaseQuantity[];

                foreach (var innerQuantity in internalQuantities)
                {
                    //try to get the quantity direct unit
                    var l2_InnerUnitType = GetDefaultSIUnitTypeOf(innerQuantity.GetType());

                    if (l2_InnerUnitType == null)
                    {
                        //this means for this quantity there is no direct mapping to SI Unit
                        // so we should create unit for this quantity

                        var un = DiscoverUnit(innerQuantity);
                        if (un.SubUnits != null && un.SubUnits.Count > 0)
                        {
                            subUnits.AddRange(un.SubUnits);
                        }
                        else
                        {
                            subUnits.Add(un);
                        }
                    }
                    else
                    {
                        //found :) create it with the exponent

                        var un = (Unit)Activator.CreateInstance(l2_InnerUnitType);
                        un.UnitExponent = innerQuantity.Exponent;
                        un.UnitDimension = innerQuantity.Dimension;

                        subUnits.Add(un);
                    }
                }
            }
            else
            {
                //subclass of AnyQuantity
                //use direct mapping with the exponent of the quantity

                var un = (Unit)Activator.CreateInstance(innerUnitType);
                un.UnitExponent = quantity.Exponent;
                un.UnitDimension = quantity.Dimension;

                return un;
            }

            return new Unit(m_QuantityType, subUnits.ToArray());
        }

        /// <summary>
        /// This constructor creates a unit from several units.
        /// </summary>
        /// <param name="quantityType"></param>
        /// <param name="units"></param>
        internal Unit(Type quantityType, params Unit[] units)
        {
            SubUnits = new List<Unit>();

            foreach (var un in units)
            {
                SubUnits.Add(un);
            }

            SubUnits = GroupUnits(SubUnits); //group similar units

            _Symbol = GenerateUnitSymbolFromSubBaseUnits();

            // if the passed type is AnyQuantity<object> for example
            //     then I want to get the type without type parameters AnyQuantity<>
            if (quantityType != null)
            {
                if (!quantityType.IsGenericTypeDefinition)
                    quantityType = quantityType.GetGenericTypeDefinition();
            }

            if (quantityType != typeof(DerivedQuantity<>) && quantityType != null)
            {
                if (quantityType != typeof(DimensionlessQuantity<>)) _IsDefaultUnit = true;

                _QuantityType = quantityType;

                //get the unit dimension from the passed type.
                _UnitDimension = QuantityDimension.DimensionFrom(quantityType);
            }
            else
            {
                //passed type is derivedQuantity which indicates that the units representing unknow derived quantity to the system
                //so that quantityType should be kept as derived quantity type.
                _QuantityType = quantityType;

                //get the unit dimension from the passed units.
                _UnitDimension = QuantityDimension.Dimensionless;
                foreach (var uu in SubUnits)
                    _UnitDimension += uu.UnitDimension;
            }

            IsBaseUnit = false;
        }

        #endregion

        /// <summary>
        /// Take the sub units recursively and return all of in a flat list.
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        private static List<Unit> FlattenUnits(List<Unit> units)
        {
            var all = new List<Unit>();
            foreach (var un in units)
            {
                if (un.IsStronglyTyped)
                    all.Add(un);
                else
                    all.AddRange(un.SubUnits);
            }
            return all;
        }

        /// <summary>
        /// Group all similar units so it remove units that reached exponent zero
        /// also keep track of prefixes of metric units.
        /// </summary>
        /// <param name="bulk_units"></param>
        /// <returns></returns>
        private List<Unit> GroupUnits(List<Unit> bulk_units)
        {
            var units = FlattenUnits(bulk_units);

            if (units.Count == 1) return units;

            var groupedUnits = new List<Unit>();

            var us = new Dictionary<Type, Unit>();
            foreach (var un in units)
            {

                if (us.ContainsKey(un.GetType()))
                {
                    //check for prefixes before accumulating units
                    //   otherwise I'll lose the UnitExponent value.
                    if (un is MetricUnit unit)
                    {
                        //check prefixes to consider milli+Mega for example for overflow

                        var accumPrefix = ((MetricUnit)us[unit.GetType()]).UnitPrefix;
                        var sourcePrefix = unit.UnitPrefix;

                        try
                        {
                            //Word about MetricPrefix
                            //   The prefix takes the unit exponent as another exponent to it
                            //  so if we are talking about cm^2 actually it is c^2*m^2
                            //  suppose we multiply cm*cm this will give cm^2
                            //     so no need to alter the prefix value
                            // however remain a problem of different prefixes
                            // for example km * cm = ?m^2
                            //  k*c = ?^2
                            //    so ? = (k+c)/2  ;)
                            //  if there is a fraction remove the prefixes totally and substitute them 
                            //  in the overflow flag.

                            // about division
                            // km / cm = ?<1>
                            // k/c = ?   or in exponent k-c=?


                            double targetExponent = us[unit.GetType()].UnitExponent + un.UnitExponent;

                            double accumExponent = accumPrefix.Exponent * us[unit.GetType()].UnitExponent;
                            double sourceExponent = sourcePrefix.Exponent * un.UnitExponent;

                            var resultExponent = (accumExponent + sourceExponent);

                            if (!(us[unit.GetType()].IsInverted ^ un.IsInverted))
                            {
                                //multiplication


                                if (resultExponent % targetExponent == 0)
                                {
                                    //we can get the symbol of the sqrt of this
                                    var unknown = resultExponent / targetExponent;

                                    ((MetricUnit)us[unit.GetType()]).UnitPrefix = MetricPrefix.FromExponent(unknown);
                                }
                                else
                                {
                                    //we can't get the approriate symbol because we have a fraction
                                    // like  kilo * centi = 3-2=1    1/2=0.5   or 1%2=1 
                                    // so we will take the whole fraction and make an overflow

                                    ((MetricUnit)us[unit.GetType()]).UnitPrefix = MetricPrefix.None;

                                    if (resultExponent != 0)
                                    {
                                        UnitOverflow += Math.Pow(10, resultExponent);
                                        _IsOverflowed = true;
                                    }
                                }
                            }
                            else
                            {
                                //division
                                //resultExponent = (accumExponent - sourceExponent);

                                ((MetricUnit)us[unit.GetType()]).UnitPrefix = MetricPrefix.None;

                                if (resultExponent != 0)   //don't overflow in case of zero exponent target because there is not prefix in this case
                                {
                                    UnitOverflow += Math.Pow(10, resultExponent);
                                    _IsOverflowed = true;
                                }

                            }
                        }
                        catch (MetricPrefixException mpe)
                        {
                            ((MetricUnit)us[unit.GetType()]).UnitPrefix = mpe.CorrectPrefix;
                            UnitOverflow += Math.Pow(10, mpe.OverflowExponent);
                            _IsOverflowed = true;
                        }
                    }

                    us[un.GetType()].UnitExponent += un.UnitExponent;
                    us[un.GetType()].UnitDimension += un.UnitDimension;
                }
                else
                {
                    us[un.GetType()] = (Unit)un.MemberwiseClone();
                }
            }

            foreach (var un in us.Values)
            {
                if (un.UnitExponent != 0)
                {
                    groupedUnits.Add(un);
                }
                else
                {
                    //zero means units should be skipped
                    // however we are testing for prefix if the unit is metric
                    //  if the unit is metric and deprecated the prefix should be taken into consideration
                    if (un is MetricUnit)
                    {
                        var mu = (MetricUnit)un;
                        if (mu.UnitPrefix.Exponent != 0)
                        {
                            _IsOverflowed = true;
                            UnitOverflow += Math.Pow(10, mu.UnitPrefix.Exponent);
                        }
                    }
                }
            }

            return groupedUnits;
        }

        protected bool _IsOverflowed;

        /// <summary>
        /// Overflow flag.
        /// </summary>
        public bool IsOverflowed => _IsOverflowed;

        protected double UnitOverflow;
        /// <summary>
        /// This method get the overflow from multiplying/divding metric units with different 
        /// prefixes and then the unit exponent goes to ZERO
        ///     or when result prefix is over the 
        /// the value should be used to be multiplied by the quantity that units were associated to.
        /// after the execution of this method the overflow flag is reset again.
        /// </summary>
        public double GetUnitOverflow()
        {
            var u = UnitOverflow;
            UnitOverflow = 0.0;
            _IsOverflowed = false;
            return u;
        }

        /// <summary>
        /// adjust the symbol string.
        /// </summary>
        /// <returns></returns>
        private string GenerateUnitSymbolFromSubBaseUnits()
        {
            var unitNumerator = "";
            var unitDenominator = "";

            void ConcatenateUnit(string symbol, float exponent)
            {
                if (exponent > 0)
                {
                    if (unitNumerator.Length > 0) unitNumerator += ".";

                    unitNumerator += symbol;

                    if (exponent > 1) unitNumerator += "^" + exponent.ToString(CultureInfo.InvariantCulture);
                    if (exponent < 1 && exponent > 0) unitNumerator += "^" + exponent.ToString(CultureInfo.InvariantCulture);
                }

                if (exponent < 0)
                {
                    if (unitDenominator.Length > 0) unitDenominator += ".";

                    unitDenominator += symbol;

                    //validate less than -1 
                    if (exponent < -1) unitDenominator += "^" + Math.Abs(exponent).ToString(CultureInfo.InvariantCulture);

                    //validate between -1 and 0
                    if (exponent > -1 && exponent < 0) unitDenominator += "^" + Math.Abs(exponent).ToString(CultureInfo.InvariantCulture);
                }
            }
            
            foreach (var unit in SubUnits)
            {
                ConcatenateUnit(unit.Symbol, unit.UnitExponent);
            }

            //return <UnitNumerator / UnitDenominator>
            string FormatUnitSymbol()
            {
                var unitSymbol = "<";

                if (unitNumerator.Length > 0)
                    unitSymbol += unitNumerator;
                else
                    unitSymbol += "1";

                if (unitDenominator.Length > 0) unitSymbol += "/" + unitDenominator;

                unitSymbol += ">";

                return unitSymbol;
            }

            var preFinalSymbol = FormatUnitSymbol();

            var finalSymbol = preFinalSymbol;

            //remove .<1/.  to be 
            const string pattern = @"\.<1/(.+?)>";

            var m = Regex.Match(preFinalSymbol, pattern);

            while (m.Success)
            {
                finalSymbol = finalSymbol.Replace(m.Groups[0].Value, "/" + m.Groups[1].Value);
                m = m.NextMatch();
            }

            return finalSymbol;
        }

        protected string _Symbol;
        protected bool _IsDefaultUnit;
        protected Type _QuantityType;
        protected QuantityDimension _UnitDimension;
        protected readonly Unit _ReferenceUnit;
        protected readonly double _ReferenceUnitNumerator;
        protected readonly double _ReferenceUnitDenominator;

        private struct UnitValues
        {
            public string Symbol;
            public bool IsDefaultUnit;
            public bool IsBaseUnit;
            public Type QuantityType;
            public QuantityDimension UnitDimension;
            public Unit ReferenceUnit;
            public double ReferenceUnitNumerator;
            public double ReferenceUnitDenominator;
        }

        private static readonly Dictionary<Type, UnitValues> CachedUnitsValues = new Dictionary<Type, UnitValues>();

        /// <summary>
        /// Fill the instance of the unit with the attributes
        /// found on it.
        /// </summary>
        protected Unit()
        {
            //only called on the strongly typed units
            IsStronglyTyped = true;


            if (CachedUnitsValues.TryGetValue(GetType(), out var uv))
            {
                _Symbol = uv.Symbol;
                _QuantityType = uv.QuantityType;
                _UnitDimension = uv.UnitDimension;
                _IsDefaultUnit = uv.IsDefaultUnit;
                IsBaseUnit = uv.IsBaseUnit;
                _ReferenceUnit = uv.ReferenceUnit;
                _ReferenceUnitNumerator = uv.ReferenceUnitNumerator;
                _ReferenceUnitDenominator = uv.ReferenceUnitDenominator;
            }
            else
            {
                //read the current attributes

                MemberInfo info = GetType();

                var attributes = info.GetCustomAttributes(true);

                //get the UnitAttribute
                var ua = (UnitAttribute)attributes.SingleOrDefault(ut => ut is UnitAttribute);

                if (ua != null)
                {
                    _Symbol = ua.Symbol;
                    _QuantityType = ua.QuantityType;
                    _UnitDimension = QuantityDimension.DimensionFrom(_QuantityType);


                    if (ua is DefaultUnitAttribute)
                    {
                        _IsDefaultUnit = true;  //indicates that this unit is the default when creating the quantity in this system
                        //also default unit is the unit that relate its self to the SI Unit.
                    }
                    else
                    {
                        _IsDefaultUnit = false;
                    }
                }
                else
                {
                    throw new UnitException("Unit Attribute not found");
                }

                if (_QuantityType.Namespace == "QuantitySystem.Quantities.BaseQuantities")
                {
                    IsBaseUnit = true;
                }
                else
                {
                    IsBaseUnit = false;
                }

                //Get the reference attribute
                var dua = (ReferenceUnitAttribute)attributes.SingleOrDefault(ut => ut is ReferenceUnitAttribute);

                if (dua != null)
                {
                    if (dua.UnitType != null)
                    {
                        _ReferenceUnit = (Unit)Activator.CreateInstance(dua.UnitType);
                    }
                    else
                    {
                        //get the SI Unit Type for this quantity
                        //first search for direct mapping
                        var siUnitType = GetDefaultSIUnitTypeOf(_QuantityType);
                        if (siUnitType != null)
                        {
                            _ReferenceUnit = (Unit)Activator.CreateInstance(siUnitType);
                        }
                        else
                        {
                            //try dynamic creation of the unit.
                            _ReferenceUnit = new Unit(_QuantityType);
                        }
                    }

                    _ReferenceUnitNumerator = dua.Numerator;
                    _ReferenceUnitDenominator = dua.Denominator;
                }

                uv.Symbol = _Symbol;
                uv.QuantityType = _QuantityType;
                uv.UnitDimension = _UnitDimension;
                uv.IsDefaultUnit = _IsDefaultUnit;
                uv.IsBaseUnit = IsBaseUnit;
                uv.ReferenceUnit = _ReferenceUnit;
                uv.ReferenceUnitNumerator = _ReferenceUnitNumerator;
                uv.ReferenceUnitDenominator = _ReferenceUnitDenominator;

                CachedUnitsValues.Add(GetType(), uv);
            }
        }

        public virtual string Symbol => _Symbol;

        /// <summary>
        /// Determine if the unit is the default unit for the quantity type.
        /// </summary>
        public virtual bool IsDefaultUnit => _IsDefaultUnit;

        /// <summary>
        /// The dimension that this unit represents.
        /// </summary>
        public QuantityDimension UnitDimension
        {
            get => _UnitDimension;
            internal set { _UnitDimension = value; }
        }


        /// <summary>
        /// The Type of the Quantity of this unit.
        /// </summary>
        public Type QuantityType
        {
            get => _QuantityType;
            internal set
            {
                _QuantityType = value;
                _UnitDimension = QuantityDimension.DimensionFrom(value);
            }
        }

        /// <summary>
        /// Tells if Unit is related to one of the seven base quantities.
        /// </summary>
        public bool IsBaseUnit { get; }


        /// <summary>
        /// The unit that serve a parent for this unit.
        /// and should take the same exponent of the unit.
        /// </summary>
        public virtual Unit ReferenceUnit
        {
            get
            {
                if (_ReferenceUnit != null)
                {
                    if (_ReferenceUnit.UnitExponent != UnitExponent)
                        _ReferenceUnit.UnitExponent = UnitExponent;
                }

                return _ReferenceUnit;
            }
        }

        /// <summary>
        /// How much the current unit equal to the reference unit.
        /// </summary>
        public double ReferenceUnitTimes => ReferenceUnitNumerator / ReferenceUnitDenominator;

        public virtual double ReferenceUnitNumerator => Math.Pow(_ReferenceUnitNumerator, UnitExponent);

        public virtual double ReferenceUnitDenominator => Math.Pow(_ReferenceUnitDenominator, UnitExponent);

        /// <summary>
        /// Invert the current unit simply from numerator to denominator and vice versa.
        /// </summary>
        /// <returns></returns>
        public Unit Invert()
        {
            Unit unit = null;
            if (SubUnits != null)
            {
                //convert sub units if this were only a generated unit.

                var InvertedUnits = new List<Unit>();

                foreach (var lun in SubUnits)
                {
                    InvertedUnits.Add(lun.Invert());

                }

                unit = new Unit(QuantityType, InvertedUnits.ToArray());

            }
            else
            {
                //convert exponent because this is a strongly typed unit.

                unit = (Unit)MemberwiseClone();
                unit.UnitExponent = 0 - UnitExponent;
                unit.UnitDimension = unit.UnitDimension.Invert();


            }
            return unit;
        }

        //I want to get away from Activator.CreateInstance because it is very slow :)
        // so I'll cach resluts 
        private static readonly Dictionary<Type, object> Qcach = new Dictionary<Type, object>();

        /// <summary>
        /// Gets the quantity of this unit based on the desired container.
        /// <see cref="QuantityType"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public AnyQuantity<T> GetThisUnitQuantity<T>()
        {

            AnyQuantity<T> Quantity = null;

            var qt = QuantityType.MakeGenericType(typeof(T));

            if (Qcach.TryGetValue(qt, out var j))
            {
                Quantity = ((AnyQuantity<T>)j).Clone();  //optimization for created quantities before

            }
            else
            {
                Quantity = (AnyQuantity<T>)Activator.CreateInstance(QuantityType.MakeGenericType(typeof(T)));
                Qcach.Add(qt, Quantity);

            }

            Quantity.Unit = this;


            return Quantity;
        }

        public AnyQuantity<T> GetThisUnitQuantity<T>(T value)
        {

            AnyQuantity<T> Quantity = null;
            if (QuantityType != typeof(DerivedQuantity<>) && QuantityType != null)
            {
                var qt = QuantityType.MakeGenericType(typeof(T));

                if (Qcach.TryGetValue(qt, out var j))
                {

                    Quantity = ((AnyQuantity<T>)j).Clone();  //optimization for created quantities before
                }
                else
                {

                    Quantity = (AnyQuantity<T>)Activator.CreateInstance(qt);

                    Qcach.Add(qt, Quantity);
                }
            }
            else
            {
                //create it from the unit dimension
                Quantity = new DerivedQuantity<T>(UnitDimension);

            }
            Quantity.Unit = this;

            Quantity.Value = value;

            if (IsOverflowed) Quantity.Value =
                AnyQuantity<T>.MultiplyScalarByGeneric(GetUnitOverflow(), value);

            return Quantity;
        }

        public float UnitExponent { get; set; } = 1;

        public const string MixedSystem = "MixedSystem";

        public string UnitSystem
        {
            get
            {
                //based on the current namespace of the unit
                //return the text of the namespace 
                // after Unit.

                if (IsStronglyTyped)
                {
                    var UnitType = GetType();

                    var ns = UnitType.Namespace.Substring(UnitType.Namespace.LastIndexOf("Units.", StringComparison.Ordinal) + 6);
                    return ns;
                }
                //mixed system
                // check all sub units if there unit system is the same then
                //  return it 

                if (SubUnits.Count > 0)
                {
                    var ns = SubUnits[0].UnitSystem;

                    var suidx = 1;

                    while (suidx < SubUnits.Count)
                    {
                        if (SubUnits[suidx].UnitSystem != ns && SubUnits[suidx].UnitSystem != "Shared")
                        {
                            ns = MixedSystem;
                            break;
                        }
                        suidx++;
                    }

                    return ns;
                }

                return "Unknown";
            }
        }

        /// <summary>
        /// Determine if the unit is inverted or not.
        /// </summary>
        public bool IsInverted
        {
            get
            {
                if (UnitExponent < 0) return true;
                return false;
            }
        }

        public bool IsStronglyTyped { get; }

        public override string ToString()
        {
            return GetType().Name + " " + Symbol;
        }

        public Unit Clone()
        {
            return (Unit)MemberwiseClone();
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Unit u)
            {
                if (Symbol.Equals(u.Symbol, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Symbol.GetHashCode();
        }

        public static bool operator ==(Unit lhs, Unit rhs)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (lhs is null || rhs is null)
            {
                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(Unit lhs, Unit rhs)
        {
            return !(lhs == rhs);
        }
    }
}
