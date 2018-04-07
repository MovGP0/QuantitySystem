using System;

namespace QuantitySystem.Units
{
    public struct MetricPrefix
    {
        public string Prefix { get; }
        public string Symbol { get; }

        public MetricPrefix(string prefix, string symbol, int exponent)
        {
            Prefix = prefix;
            Symbol = symbol;
            Exponent = exponent;
        }

        /// <summary>
        /// Gets the factor to convert to the required prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns>Conversion factor</returns>
        public double GetFactorForConvertTo(MetricPrefix prefix)
        {
            return prefix.Factor / Factor;
        }

        public static MetricPrefix Yotta => new MetricPrefix("yotta", "Y", 24);
        public static MetricPrefix Zetta => new MetricPrefix("zetta", "Z", 21);
        public static MetricPrefix Exa => new MetricPrefix("exa", "E", 18);
        public static MetricPrefix Peta => new MetricPrefix("peta", "P", 15);
        public static MetricPrefix Tera => new MetricPrefix("tera", "T", 12);
        public static MetricPrefix Giga => new MetricPrefix("giga", "G", 9);
        public static MetricPrefix Mega => new MetricPrefix("mega", "M", 6);
        public static MetricPrefix Kilo => new MetricPrefix("kilo", "k", 3);
        public static MetricPrefix Hecto => new MetricPrefix("hecto", "h", 2);
        public static MetricPrefix Deka => new MetricPrefix("deka", "da", 1);
        public static MetricPrefix None => new MetricPrefix("", "", 0);
        public static MetricPrefix Deci => new MetricPrefix("deci", "d", -1);
        public static MetricPrefix Centi => new MetricPrefix("centi", "c", -2);
        public static MetricPrefix Milli => new MetricPrefix("milli", "m", -3);
        public static MetricPrefix Micro => new MetricPrefix("micro", "µ", -6);
        public static MetricPrefix Nano => new MetricPrefix("nano", "n", -9);
        public static MetricPrefix Pico => new MetricPrefix("pico", "p", -12);
        public static MetricPrefix Femto => new MetricPrefix("femto", "f", -15);
        public static MetricPrefix Atto => new MetricPrefix("atto", "a", -18);
        public static MetricPrefix Zepto => new MetricPrefix("zepto", "z", -21);
        public static MetricPrefix Yocto => new MetricPrefix("yocto", "y", -24);
        
        public static MetricPrefix GetPrefix(int index)
        {
            switch (index)
            {
                case 10: return Yotta;
                case 9: return Zetta;
                case 8: return Exa;
                case 7: return Peta;
                case 6: return Tera;
                case 5: return Giga;
                case 4: return Mega;
                case 3: return Kilo;
                case 2: return Hecto;
                case 1: return Deka;
                case 0: return None;
                case -1: return Deci;
                case -2: return Centi;
                case -3: return Milli;
                case -4: return Micro;
                case -5: return Nano;
                case -6: return Pico;
                case -7: return Femto;
                case -8: return Atto;
                case -9: return Zepto;
                case -10: return Yocto;
            }

            throw new MetricPrefixException("Index out of range");
        }

        public static MetricPrefix FromExponent(double exponent)
        {
            CheckExponent(exponent);
            var exp = (int)exponent; 
            switch (exp)
            {
                case -24: return Yocto;
                case -21: return Zepto;
                case -18: return Atto;
                case -15: return Femto;
                case -12: return Pico;
                case -9: return Nano;
                case -6: return Micro;
                case -3: return Milli;
                case -2: return Centi;
                case -1: return Deci;
                case 0: return None;
                case 1: return Deka;
                case 2: return Hecto;
                case 3: return Kilo;
                case 6: return Mega;
                case 9: return Giga;
                case 12: return Tera;
                case 15: return Peta;
                case 18: return Exa;
                case 21: return Zetta;
                case 24: return Yotta;
                default: throw new MetricPrefixException("No SI Prefix found.") { WrongExponent = (int)exponent };
            }
        }

        public static MetricPrefix FromPrefixName(string prefixName)
        {
            switch (prefixName.ToLowerInvariant())
            {
                case "yocto": return Yocto;
                case "zepto": return Zepto;
                case "atto": return Atto;
                case "femto": return Femto;
                case "pico": return Pico;
                case "nano": return Nano;
                case "micro": return Micro;
                case "milli": return Milli;
                case "centi": return Centi;
                case "deci": return Deci;
                case "none": return None;
                case "deka": return Deka;
                case "hecto": return Hecto;
                case "kilo": return Kilo;
                case "mega": return Mega;
                case "giga": return Giga;
                case "tera": return Tera;
                case "peta": return Peta;
                case "exa": return Exa;
                case "zetta": return Zetta;
                case "yotta": return Yotta;
                default: throw new MetricPrefixException("No SI Prefix found for prefix = " + prefixName);
            }
        }

        public int Exponent { get; }

        public double Factor => Math.Pow(10, Exponent);

        public MetricPrefix Invert()
        {
            return FromExponent(0 - Exponent);
        }

        public static MetricPrefix Add(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
        {
            var exp = firstPrefix.Exponent + secondPrefix.Exponent;
            CheckExponent(exp);
            return FromExponent(exp);
        }

        public static MetricPrefix operator +(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
        {
            return Add(firstPrefix, secondPrefix);
        }        
        
        public static MetricPrefix Subtract(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
        {
            var exp = firstPrefix.Exponent - secondPrefix.Exponent;
            CheckExponent(exp);
            return FromExponent(exp);
        }

        public static MetricPrefix operator -(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
        {
            return Subtract(firstPrefix, secondPrefix);
        }

        public static MetricPrefix Multiply(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
        {
            var exp = firstPrefix.Exponent * secondPrefix.Exponent;
            CheckExponent(exp);
            return FromExponent(exp);
        }

        public static MetricPrefix operator *(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
        {
            return Multiply(firstPrefix, secondPrefix);
        }

        public static MetricPrefix Divide(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
        {
            var exp = firstPrefix.Exponent / secondPrefix.Exponent;
            CheckExponent(exp);
            return FromExponent(exp);
        }

        public static MetricPrefix operator /(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
        {
            return Divide(firstPrefix, secondPrefix);
        }

        /// <summary>
        /// Check the exponent if it can found or
        /// if it exceeds 24 or precedes -25 <see cref="MetricPrefixException"/> occur with the closest 
        /// <see cref="MetricPrefix"/> and overflow the rest of it.
        /// </summary>
        /// <param name="expo"></param>
        public static void CheckExponent(double expo)
        {
            var exp = (int)Math.Floor(expo);

            var ov = expo - exp;

            if (exp > 24) throw new MetricPrefixException("Exponent Exceed 24")
            {
                WrongExponent = exp,
                CorrectPrefix = FromExponent(24),
                OverflowExponent = (exp - 24) + ov
            };

            if (exp < -24) throw new MetricPrefixException("Exponent Precede -24")
            {
                WrongExponent = exp,
                CorrectPrefix = FromExponent(-24),
                OverflowExponent = (exp + 24) + ov
            };

            int[] wrongexp = { 4, 5, 7, 8, 10, 11, 13, 14, 16, 17, 19, 20, 22, 23 };
            int[] correctexp = { 3, 3, 6, 6, 9, 9, 12, 12, 15, 15, 18, 18, 21, 21 };

            for (var i = 0; i < wrongexp.Length; i++)
            {
                //find if exponent in wrong powers
                if (Math.Abs(exp) == wrongexp[i])
                {
                    var cexp = 0;
                    if (exp > 0) cexp = correctexp[i];
                    if (exp < 0) cexp = -1 * correctexp[i];

                    throw new MetricPrefixException("Exponent not aligned")
                    {
                        WrongExponent = exp,
                        CorrectPrefix = FromExponent(cexp),
                        OverflowExponent = (exp - cexp) + ov           //5-3 = 2  ,  -5--3 =-2

                    };
                }
            }

            if (ov != 0)
            {
                //then the exponent must be 0.5
                throw new MetricPrefixException("Exponent not aligned")
                {
                    WrongExponent = exp,
                    CorrectPrefix = FromExponent(0),
                    OverflowExponent = ov           //5-3 = 2  ,  -5--3 =-2
                };
            }
        }
    }
}
