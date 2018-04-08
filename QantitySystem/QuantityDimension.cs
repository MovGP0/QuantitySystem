using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using QuantitySystem.DimensionDescriptors;
using QuantitySystem.Exceptions;
using QuantitySystem.Quantities;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem
{
    /// <summary>
    /// Quantity Dimension.
    /// dim Q = Lα Mβ Tγ Iδ Θε Nζ Jη
    /// </summary>
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public sealed class QuantityDimension
    {
        public MassDescriptor Mass { get; set; }
        public LengthDescriptor Length { get; set; }
        public TimeDescriptor Time { get; set; }
        public ElectricCurrentDescriptor ElectricCurrent { get; set; }
        public TemperatureDescriptor Temperature { get; set; }
        public AmountOfSubstanceDescriptor AmountOfSubstance { get; set; }
        public LuminousIntensityDescriptor LuminousIntensity { get; set; }
        public CurrencyDescriptor Currency { get; set; }
        public DigitalDescriptor Digital { get; set; }

        public QuantityDimension()
        {
        }

        /// <summary>
        /// basic constructor for MLT Dimensions.
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="length"></param>
        /// <param name="time"></param>
        public QuantityDimension(float mass, float length, float time)
        {
            Mass = new MassDescriptor(mass);
            Length = new LengthDescriptor(length, 0);
            Time = new TimeDescriptor(time);
        }

        public QuantityDimension(float mass, float length, float time, float temperature)
        {
            Mass = new MassDescriptor(mass);
            Length = new LengthDescriptor(length, 0);
            Time = new TimeDescriptor(time);
            Temperature = new TemperatureDescriptor(temperature);
        }

        public QuantityDimension(float mass, float length, float time, float temperature, float electricalCurrent, float amountOfSubstance, float luminousIntensity)
        {
            Mass = new MassDescriptor(mass);
            Length = new LengthDescriptor(length, 0);
            Time = new TimeDescriptor(time);
            Temperature = new TemperatureDescriptor(temperature);
            ElectricCurrent = new ElectricCurrentDescriptor( electricalCurrent);
            AmountOfSubstance = new AmountOfSubstanceDescriptor(amountOfSubstance);
            LuminousIntensity = new LuminousIntensityDescriptor(luminousIntensity);
        }

        /// <summary>
        /// The Quality in MLT form
        /// </summary>
        public string MLT
        {
            get
            {
                var mass = "M" + Mass.Exponent.ToString(CultureInfo.InvariantCulture);
                var length = "L" + Length.Exponent.ToString(CultureInfo.InvariantCulture);
                var time = "T" + Time.Exponent.ToString(CultureInfo.InvariantCulture);
                return mass + length + time;
            }
        }
        
        /// <summary>
        /// returns the power of Force.
        /// </summary>
        public float ForceExponent
        {
            get
            {
                var targetM = Mass.Exponent;
                float targetF = 0;
                while (targetM > 0)
                {
                    targetM--;
                    targetF++;
                }

                return targetF;
            }
        }

        /// <summary>
        /// FLT powers based on Force Length, and Time.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FLT")]
        public string FLT
        {
            get
            {
                var targetM = Mass.Exponent;
                var targetL = Length.Exponent;
                var targetT = Time.Exponent;
                float targetF = 0;

                while (targetM > 0)
                {
                    targetM--;
                    targetF++;

                    targetL -= 1;
                    targetT += 2;
                }

                var force = "F" + targetF.ToString(CultureInfo.InvariantCulture);
                var length = "L" + targetL.ToString(CultureInfo.InvariantCulture);
                var time = "T" + targetT.ToString(CultureInfo.InvariantCulture);

                return force + length + time;
            }
        }
        
        public override string ToString()
        {
            var dim = "";

            dim += "M" + Mass.Exponent.ToString(CultureInfo.InvariantCulture);

            if (Length.PolarExponent != 0)
            {
                dim +=
                    string.Format("L{0}(RL{1}PL{2})",
                    Length.Exponent.ToString(CultureInfo.InvariantCulture),
                    Length.RegularExponent.ToString(CultureInfo.InvariantCulture),
                    Length.PolarExponent.ToString(CultureInfo.InvariantCulture)
                    );
            }
            else
            {
                dim += string.Format("L{0}",
                    Length.Exponent.ToString(CultureInfo.InvariantCulture));
            }
            
            dim += "T" + Time.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "I" + ElectricCurrent.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "O" + Temperature.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "N" + AmountOfSubstance.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "J" + LuminousIntensity.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "$" + Currency.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "D" + Digital.Exponent.ToString(CultureInfo.InvariantCulture);

            return dim;
        }

        public override bool Equals(object obj)
        {
            if (obj is QuantityDimension qd)
            {
                if (!ElectricCurrent.Equals(qd.ElectricCurrent))
                    return false;

                if (!Length.Equals(qd.Length))
                    return false;
                
                if (!LuminousIntensity.Equals( qd.LuminousIntensity))
                    return false;

                if (!Mass.Equals(qd.Mass))
                    return false;

                if (!AmountOfSubstance.Equals(qd.AmountOfSubstance))
                    return false;

                if (!Temperature.Equals(qd.Temperature))
                    return false;

                if (!Time.Equals(qd.Time))
                    return false;

                if (!Currency.Equals(qd.Currency))
                    return false;

                if (!Digital.Equals(qd.Digital))
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Equality here based on first level of exponent validation.
        /// Which means length explicitly is compared to the total dimension value not on radius and normal length values.
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public bool IsEqual(QuantityDimension dimension)
        {
            if (ElectricCurrent.Exponent != dimension.ElectricCurrent.Exponent)
                return false;

            if (Length.Exponent != dimension.Length.Exponent)
                return false;

            if (LuminousIntensity.Exponent != dimension.LuminousIntensity.Exponent)
                return false;

            if (Mass.Exponent != dimension.Mass.Exponent)
                return false;

            if (AmountOfSubstance.Exponent != dimension.AmountOfSubstance.Exponent)
                return false;

            if (Temperature.Exponent != dimension.Temperature.Exponent)
                return false;

            if (Time.Exponent != dimension.Time.Exponent)
                return false;

            if (Currency.Exponent != dimension.Currency.Exponent)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        
        public static QuantityDimension operator +(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            return Add(firstDimension, secondDimension);
        }

        public static QuantityDimension Add(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            return new QuantityDimension
            {
                Mass = firstDimension.Mass.Add(secondDimension.Mass),
                Length = firstDimension.Length.Add(secondDimension.Length),
                Time = firstDimension.Time.Add(secondDimension.Time),
                Temperature = firstDimension.Temperature.Add(secondDimension.Temperature),
                ElectricCurrent = firstDimension.ElectricCurrent.Add(secondDimension.ElectricCurrent),
                AmountOfSubstance = firstDimension.AmountOfSubstance.Add(secondDimension.AmountOfSubstance),
                LuminousIntensity = firstDimension.LuminousIntensity.Add(secondDimension.LuminousIntensity),
                Currency = firstDimension.Currency.Add(secondDimension.Currency),
                Digital = firstDimension.Digital.Add(secondDimension.Digital)
            };
        }

        public static QuantityDimension operator -(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            return Subtract(firstDimension, secondDimension);
        }

        public static QuantityDimension Subtract(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            return new QuantityDimension
            {
                Mass = firstDimension.Mass.Subtract(secondDimension.Mass),
                Length = firstDimension.Length.Subtract(secondDimension.Length),
                Time = firstDimension.Time.Subtract(secondDimension.Time),
                Temperature = firstDimension.Temperature.Subtract(secondDimension.Temperature),
                ElectricCurrent = firstDimension.ElectricCurrent.Subtract(secondDimension.ElectricCurrent),
                AmountOfSubstance = firstDimension.AmountOfSubstance.Subtract(secondDimension.AmountOfSubstance),
                LuminousIntensity = firstDimension.LuminousIntensity.Subtract(secondDimension.LuminousIntensity),
                Currency = firstDimension.Currency.Subtract(secondDimension.Currency),
                Digital = firstDimension.Digital.Subtract(secondDimension.Digital)
            };
        }

        public static QuantityDimension operator *(QuantityDimension dimension, float exponent)
        {
            return Multiply(dimension, exponent);
        }

        public static QuantityDimension Multiply(QuantityDimension dimension, float exponent)
        {
            return new QuantityDimension
            {
                Mass = dimension.Mass.Multiply(exponent),
                Length = dimension.Length.Multiply(exponent),
                Time = dimension.Time.Multiply(exponent),
                Temperature = dimension.Temperature.Multiply(exponent),
                ElectricCurrent = dimension.ElectricCurrent.Multiply(exponent),
                AmountOfSubstance = dimension.AmountOfSubstance.Multiply(exponent),
                LuminousIntensity = dimension.LuminousIntensity.Multiply(exponent),
                Currency = dimension.Currency.Multiply(exponent),
                Digital = dimension.Digital.Multiply(exponent)
            };
        }
        
        private static readonly List<Type> CurrentQuantityTypes = new List<Type>();

        /// <summary>
        /// holding Dimension -> Quantity instance  to be clonned.
        /// </summary>
        private static readonly Dictionary<QuantityDimension, Type> CurrentQuantitiesDictionary = new Dictionary<QuantityDimension, Type>();

        /// <summary>
        /// Quantity Name -> Quantity Type  as all quantity names are unique
        /// </summary>
        private static readonly Dictionary<string, Type> CurrentQuantitiesNamesDictionary = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// holding Quantity -> Dimension
        /// </summary>
        private static readonly Dictionary<Type, QuantityDimension> CurrentDimensionsDictionary = new Dictionary<Type, QuantityDimension>();

        public static Type[] AllQuantitiesTypes => CurrentQuantitiesNamesDictionary.Values.ToArray();

        public static string[] AllQuantitiesNames => CurrentQuantitiesNamesDictionary.Keys.ToArray();

        /// <summary>
        /// Cache all quantities with their Dimensions.
        /// </summary>
        static QuantityDimension()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var types = currentAssembly.GetTypes();

            var quantityTypes = from quantityType in types
                                where quantityType.IsSubclassOf(typeof(BaseQuantity))
                                select quantityType;

            CurrentQuantityTypes.AddRange(quantityTypes);

            //storing the quantity types with thier dimensions

            foreach (var quantityType in CurrentQuantityTypes)
            {
                //cach the quantities that is not abstract types

                if (quantityType.IsAbstract == false && quantityType != typeof(DerivedQuantity<>))
                {
                    //make sure not to include Dimensionless quantities due to they are F0L0T0
                    if (quantityType.BaseType.Name != typeof(DimensionlessQuantity<>).Name)
                    {

                        //store dimension as key and Quantity Type .
                        
                        //create AnyQuantity<Object>  Object container used just for instantiation
                        var quantity = (AnyQuantity<Object>)Activator.CreateInstance(quantityType.MakeGenericType(typeof(object)));

                        //store the Dimension and the corresponding Type;
                        CurrentQuantitiesDictionary.Add(quantity.Dimension, quantityType);

                        //store quantity type as key and corresponding dimension as value.
                        CurrentDimensionsDictionary.Add(quantityType, quantity.Dimension);

                        //store the quantity name and type with insensitive names
                        CurrentQuantitiesNamesDictionary.Add(quantityType.Name.Substring(0, quantityType.Name.Length - 2), quantityType);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the quantity type or typeof(DerivedQuantity&lt;&gt;) without throwing exception
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public static Type GetQuantityTypeFrom(QuantityDimension dimension)
        {
            return CurrentQuantitiesDictionary.TryGetValue(dimension, out var qType) ? qType : typeof(DerivedQuantity<>);
        }

        /// <summary>
        /// Get the corresponding typed quantity in the framework of this dimension
        /// Throws QuantityNotFounException when there is corresponding one.
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public static Type QuantityTypeFrom(QuantityDimension dimension)
        {
            try
            {
                return CurrentQuantitiesDictionary[dimension];
            }
            catch (KeyNotFoundException ex)
            {
                throw new QuantityNotFoundException("Couldn't Find the quantity dimension in the dimensions Hash Key", ex);
            }
        }

        /// <summary>
        /// Gets the quantity type from the name and throws QuantityNotFounException if not found.
        /// </summary>
        /// <param name="quantityName"></param>
        /// <returns></returns>
        public static Type QuantityTypeFrom(string quantityName)
        {
            try
            {
                return CurrentQuantitiesNamesDictionary[quantityName];
            }
            catch (KeyNotFoundException ex)
            {
                throw new QuantityNotFoundException("Couldn't Find the quantity dimension in the dimensions Hash Key", ex);
            }
        }

        private static readonly Dictionary<Type, object> QuantitiesCached = new Dictionary<Type, object>();

        /// <summary>
        /// Returns Strongly typed Any Quantity From the dimension based on the discovered quantities discovered when 
        /// framework initiated.
        /// Throws <see cref="QuantityNotFoundException"/> when quantity is not found.
        /// </summary>
        /// <typeparam name="T">The value container of the Quantity</typeparam>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public static AnyQuantity<T> QuantityFrom<T>(QuantityDimension dimension)
        {
            lock (QuantitiesCached)
            {
                var quantityType = QuantityTypeFrom(dimension);

                //the quantity type now is without container type we should generate it

                var quantityWithContainerType = quantityType.MakeGenericType(typeof(T));

                if (QuantitiesCached.TryGetValue(quantityWithContainerType, out var j))
                {
                    return ((AnyQuantity<T>)j).Clone();
                }

                j = Activator.CreateInstance(quantityWithContainerType);
                QuantitiesCached.Add(quantityWithContainerType, j);
                return ((AnyQuantity<T>)j).Clone();
            }
        }

        /// <summary>
        /// Returns the quantity dimenstion based on the quantity type.
        /// </summary>
        /// <param name="quantityType"></param>
        /// <returns></returns>
        public static QuantityDimension DimensionFrom(Type quantityType)
        {
            if (!quantityType.IsGenericTypeDefinition)
            {
                //the passed type is AnyQuantity<object> for example
                //I want to get the type without type parameters AnyQuantity<>
                quantityType = quantityType.GetGenericTypeDefinition();
            }

            try
            {
                return CurrentDimensionsDictionary[quantityType];
            }
            catch (KeyNotFoundException ex)
            {
                //if key not found and quantityType is really Quantity
                //then return dimensionless Quantity

                if (quantityType.BaseType.GetGenericTypeDefinition() == typeof(DimensionlessQuantity<>))
                    return Dimensionless;
                throw new DimensionNotFoundException("UnExpected Exception. TypeOf<" + quantityType.ToString() + "> have no dimension associate with it", ex);
            }          
        }

        /// <summary>
        /// Extract the Mass power from MLT string.
        /// </summary>
        /// <param name="mlt"></param>
        /// <returns></returns>
        private static int ExponentOfMass(string mlt)
        {
            const int mIndex = 0;
            var lIndex = mlt.IndexOf('L');
            return int.Parse(mlt.Substring(mIndex + 1, lIndex - mIndex - 1), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Extract the Length Power from MLT string.
        /// </summary>
        /// <param name="mlt"></param>
        /// <returns></returns>
        private static int ExponentOfLength(string mlt)
        {
            var lIndex = mlt.IndexOf('L');
            var tIndex = mlt.IndexOf('T');
            return int.Parse(mlt.Substring(lIndex + 1, tIndex - lIndex - 1), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Extract the Time Power from MLT string.
        /// </summary>
        /// <param name="mlt"></param>
        /// <returns></returns>
        private static int ExponentOfTime(string mlt)
        {
            var tIndex = mlt.IndexOf('T');
            return int.Parse(mlt.Substring(tIndex + 1, mlt.Length - tIndex - 1), CultureInfo.InvariantCulture);
        }

        public static QuantityDimension ParseMLT(string mlt)
        {
            var m = ExponentOfMass(mlt);
            var l = ExponentOfLength(mlt);
            var t = ExponentOfTime(mlt);

            return new QuantityDimension(m, l, t);
        }

        public static QuantityDimension Parse(string dimension)
        {
            dimension = dimension.Trim();

            // M L T I O N J   C
            var exps = new List<float>();
            if (char.IsNumber(dimension[0]))
            {
                // parsing started with numbers which will be most probably 
                //   on the format of numbers with spaces
                var dims = dimension.Split(' ');
                foreach (var dim in dims)
                {
                    var dimtrimmed = dim.Trim();

                    if (!string.IsNullOrEmpty(dimtrimmed))
                    {
                        exps.Add(float.Parse(dimtrimmed));
                    }
                }

                var m = exps.Count > 0 ? exps[0] : 0;
                var l = exps.Count > 1 ? exps[1] : 0;
                var T = exps.Count > 2 ? exps[2] : 0;
                var I = exps.Count > 3 ? exps[3] : 0;
                var o = exps.Count > 4 ? exps[4] : 0;
                var n = exps.Count > 5 ? exps[5] : 0;
                var j = exps.Count > 6 ? exps[6] : 0;
                var c = exps.Count > 7 ? exps[7] : 0;
                var d = exps.Count > 8 ? exps[8] : 0;

                var qd = new QuantityDimension(m, l, T, I, o, n, j)
                {
                    Currency = new CurrencyDescriptor(c),
                    Digital = new DigitalDescriptor(d)
                };

                return qd;
            }

            // the format is based on letter and number

            var dumber  = dimension.ToUpperInvariant();

            var mts = Regex.Matches(dumber, @"(([MmLlTtIiOoNnJjCcDd])(\-*[0-9]+))+?");

            var edps = new Dictionary<char,float>();

            foreach(Match m in mts)
            {
                edps.Add(m.Groups[2].Value[0], float.Parse(m.Groups[3].Value));
            }

            if (!edps.ContainsKey('M')) edps['M'] = 0;
            if (!edps.ContainsKey('L')) edps['L'] = 0;
            if (!edps.ContainsKey('T')) edps['T'] = 0;
            if (!edps.ContainsKey('I')) edps['I'] = 0;
            if (!edps.ContainsKey('O')) edps['O'] = 0;
            if (!edps.ContainsKey('N')) edps['N'] = 0;
            if (!edps.ContainsKey('J')) edps['J'] = 0;
            if (!edps.ContainsKey('C')) edps['C'] = 0;
            if (!edps.ContainsKey('D')) edps['D'] = 0;

            return new QuantityDimension(edps['M'], edps['L'], edps['T'], edps['I'], edps['O'], edps['N'], edps['J'])
            {
                Currency = new CurrencyDescriptor(edps['C']),
                Digital = new DigitalDescriptor(edps['D'])
            };
        }
        
        public static QuantityDimension Dimensionless => new QuantityDimension();

        public bool IsDimensionless => Mass.Exponent == 0
                                    && Length.Exponent == 0
                                    && Time.Exponent == 0
                                    && ElectricCurrent.Exponent == 0
                                    && Temperature.Exponent == 0
                                    && AmountOfSubstance.Exponent == 0
                                    && LuminousIntensity.Exponent == 0
                                    && Currency.Exponent == 0
                                    && Digital.Exponent == 0;

        public QuantityDimension Invert()
        {
            return new QuantityDimension
            {
                Mass = Mass.Invert(),
                Length = Length.Invert(),
                Time = Time.Invert(),
                ElectricCurrent = ElectricCurrent.Invert(),
                Temperature = Temperature.Invert(),
                AmountOfSubstance = AmountOfSubstance.Invert(),
                LuminousIntensity = LuminousIntensity.Invert(),
                Currency = Currency.Invert(),
                Digital = Digital.Invert()
            };
        }

        public QuantityDimension(QuantityDimension dimension)
        {
            Mass = dimension.Mass;
            Length = dimension.Length;
            Time = dimension.Time;
            ElectricCurrent = dimension.ElectricCurrent;
            Temperature = dimension.Temperature;
            AmountOfSubstance = dimension.AmountOfSubstance;
            LuminousIntensity = dimension.LuminousIntensity;
            Currency = dimension.Currency;
            Digital = dimension.Digital;
        }
    }
}
