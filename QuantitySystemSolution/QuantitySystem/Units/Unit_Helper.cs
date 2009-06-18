﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;

namespace QuantitySystem.Units
{
    public partial class Unit
    {
        
        /// <summary>
        /// Returns quantity based on current unit instance.
        /// </summary>
        /// <typeparam name="T">Quatntity Storage Type</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        internal AnyQuantity<T> MakeQuantity<T>(T value)
        {


            //create the corresponding quantity
            AnyQuantity<T> qty = this.GetThisUnitQuantity<T>();

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
        public static AnyQuantity<double> QuantityOf<TUnit>(double value) where TUnit:Unit, new()
        {

            Unit unit = new TUnit();
            return unit.MakeQuantity<double>(value);

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
                    Type[] AllTypes = Assembly.GetExecutingAssembly().GetTypes();

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


            unitSystem = unitSystem.ToLower(CultureInfo.InvariantCulture);

            if (unitSystem.Contains("metric.si"))
            {
                Type oUnitType = GetDefaultSIUnitTypeOf(quantityType);
                return oUnitType;
            }
            else
            {
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
                    MemberInfo info = unitType as MemberInfo;

                    object[] attributes = (object[])info.GetCustomAttributes(true);

                    //get the UnitAttribute
                    UnitAttribute ua = (UnitAttribute)attributes.SingleOrDefault<object>(ut => ut is UnitAttribute);

                    if (ua != null)
                    {
                        if (ua.QuantityType == quantityType)
                        {
                            if (ua is DefaultUnitAttribute)
                            {
                                //explicitly default unit.
                                return true;
                            }
                            else if (ua is MetricUnitAttribute)
                            {
                                //check if the unit has SystemDefault flag true or not.
                                MetricUnitAttribute mua = ua as MetricUnitAttribute;
                                if (mua.SystemDefault)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                            return false;

                    }
                    else
                    {
                        return false;
                    }
                };


                string CurrentUnitSystem = unitSystem; //
                Type SystemUnitType = null;

                //search in upper namespaces also to get the default unit of the parent system.
                while (string.IsNullOrEmpty(CurrentUnitSystem) == false && SystemUnitType == null)
                {

                    //prepare the query that we will search in
                    var SystemUnitTypes = from ut in UnitTypes
                                          where ut.Namespace.ToLower(CultureInfo.InvariantCulture).EndsWith(CurrentUnitSystem)
                                                || ut.Namespace.ToLower(CultureInfo.InvariantCulture).EndsWith("shared")
                                          select ut;

                    //select the default by predictor from the query
                    SystemUnitType = SystemUnitTypes.SingleOrDefault(
                        SearchForQuantityType
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

                if (SystemUnitType == null && unitSystem.Contains("metric"))
                {
                    //try another catch for SI unit for this quantity
                    //   because SI and metric units are disordered for now
                    // so if the search of unit in parent metric doesn't exist then search for it in SI units.

                    SystemUnitType = GetDefaultSIUnitTypeOf(quantityType);
                }

                return SystemUnitType;
            }
        }


        /// <summary>
        /// Gets the unit type of quantity type parameter based on SI unit system.
        /// The function is direct mapping from types of quantities to types of units.
        /// if function returns null then this quantity dosen't have a statically linked unit to it.
        /// this means the quantity should return a unit in runtime.
        /// </summary>
        /// <param name="quantityType">Type of Quantity</param>
        /// <returns>SI Unit Type</returns>
        public static Type GetDefaultSIUnitTypeOf(Type quantityType)
        {
            //getting the generic type
            if (!quantityType.IsGenericTypeDefinition)
            {
                //the passed type is AnyQuantity<object> for example
                //I want to get the type without type parameters AnyQuantity<>

                quantityType = quantityType.GetGenericTypeDefinition();

            }

            //don't forget to include second in si units it is shared between all metric systems
            var SIUnitTypes = from si in UnitTypes
                              where si.Namespace.ToLower(CultureInfo.InvariantCulture).EndsWith("si") || si.Namespace.ToLower(CultureInfo.InvariantCulture).EndsWith("shared")
                              select si;



            Func<Type, bool> SearchForQuantityType = unitType =>
            {
                //search in the attributes of the unit type
                MemberInfo info = unitType as MemberInfo;

                object[] attributes = (object[])info.GetCustomAttributes(true);

                //get the UnitAttribute
                UnitAttribute ua = (UnitAttribute)attributes.SingleOrDefault<object>(ut => ut is UnitAttribute);

                if (ua != null)
                {
                    if (ua.QuantityType == quantityType)
                        return true;
                    else
                        return false;

                }
                else
                {
                    return false;
                }
            };


            Type SIUnitType = SIUnitTypes.SingleOrDefault(
                SearchForQuantityType
                );

            return SIUnitType;



        }

        #endregion


        /// <summary>
        /// Find Strongly typed unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private static Unit FindUnit (string unit)
        {
            foreach (Type unitType in UnitTypes)
            {
                UnitAttribute ua = GetUnitAttribute(unitType);
                if (ua != null)
                {

                    //units are case sensitive
                    if (Regex.Match(ua.Symbol, "^" + unit + "$", RegexOptions.Singleline).Success)
                    {
                        Unit u = (Unit)Activator.CreateInstance(unitType);

                        //test unit if it is metric so that we remove the default prefix that created with it
                        if (u is MetricUnit)
                        {
                            ((MetricUnit)u).UnitPrefix = MetricPrefix.None;
                        }

                        return u;

                    }
                }

            }

            throw new UnitNotFoundException("Not found in strongly typed units");
        }

        /// <summary>
        /// Returns the unit corresponding to the passed string.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static Unit Parse(string unit)
        {
            

            //Phase 1: try direct mapping.
            try
            {
                return FindUnit(unit);
            }
            catch(UnitNotFoundException)
            {
                //do nothing 
            }

            //try to find if it as a Metric unit with prefix
            //loop through all prefixes.
            for (int i = 10; i >= -10; i -= 1)
            {
                if (i == 0) i--; //skip the none prefix
                if (unit.StartsWith(MetricPrefix.GetPrefix(i).Symbol, StringComparison.InvariantCulture))
                {
                    //found

                    MetricPrefix mp = MetricPrefix.GetPrefix(i);
                    string upart = unit.Substring(mp.Symbol.Length);

                    //then it should be MetricUnit otherwise die :)

                    MetricUnit u = FindUnit(upart) as MetricUnit;

                    if (u == null) goto nounit;
                    
                    u.UnitPrefix = mp;
                    return u;
                }
            }

            nounit:
            throw new UnitNotFoundException("Not found in strongly typed units");

        }


        /// <summary>
        /// Get the unit attribute which hold the unit information.
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public static UnitAttribute GetUnitAttribute(Type unitType)
        {
            object[] attributes = (object[])unitType.GetCustomAttributes(true);

            //get the UnitAttribute
            UnitAttribute ua = (UnitAttribute)attributes.SingleOrDefault<object>(ut => ut is UnitAttribute);

            return ua;

        }



        public static Unit ExpandMetricUnit(MetricUnit unit)
        {
            List<Unit> DefaultUnits = new List<Unit>();

            if (unit.IsBaseUnit)
            {
                //if baseunit then why we would convert it
                // put it immediately
                return unit;
            }
            else
            {

                QuantityDimension qdim = QuantityDimension.DimensionFrom(unit.QuantityType);

                if (unit is MetricUnit)
                {
                    //pure unit without sub units like Pa, N, and L

                    Unit u = DiscoverUnit(qdim);

                    List<Unit> baseUnits = u.SubUnits;

                    //add prefix to the first unit in the array

                    ((MetricUnit)baseUnits[0]).UnitPrefix += ((MetricUnit)unit).UnitPrefix;

                    DefaultUnits.AddRange(baseUnits);
                }

                return new Unit(unit.quantityType, DefaultUnits.ToArray());

            }
        }


    }
}
