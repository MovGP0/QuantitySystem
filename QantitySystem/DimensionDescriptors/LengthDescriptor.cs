namespace QuantitySystem.DimensionDescriptors
{
    public struct LengthDescriptor : IDimensionDescriptor<LengthDescriptor>
    {
        public LengthDescriptor(float normalExponent, float radiusExponent):this()
        {
            RegularExponent = normalExponent;
            PolarExponent = radiusExponent;
        }
        
        public float RegularExponent { get; set; }
        public float PolarExponent { get; set; }

        public override bool Equals(object obj)
        {
            try
            {
                var ld = (LengthDescriptor)obj;
                {
                    if (RegularExponent != ld.RegularExponent) return false;

                    if (PolarExponent != ld.PolarExponent) return false;

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return RegularExponent.GetHashCode() ^ PolarExponent.GetHashCode();
        }

        public float Exponent
        {
            get => RegularExponent + PolarExponent;
            set { }
        }

        public LengthDescriptor Add(LengthDescriptor dimensionDescriptor)
        {
            return new LengthDescriptor
            {
                RegularExponent = RegularExponent + dimensionDescriptor.RegularExponent,
                PolarExponent = PolarExponent + dimensionDescriptor.PolarExponent
            };
        }

        public LengthDescriptor Subtract(LengthDescriptor dimensionDescriptor)
        {
            return new LengthDescriptor
            {
                RegularExponent = RegularExponent - dimensionDescriptor.RegularExponent,
                PolarExponent = PolarExponent - dimensionDescriptor.PolarExponent
            };
        }

        public LengthDescriptor Multiply(float exponent)
        {
            return new LengthDescriptor
            {
                RegularExponent = RegularExponent * exponent,
                PolarExponent = PolarExponent * exponent
            };
        }

        public LengthDescriptor Invert()
        {
            return new LengthDescriptor
            {
                RegularExponent = 0 - RegularExponent,
                PolarExponent = 0 - PolarExponent
            };
        }

        public static LengthDescriptor NormalLength(int exponent)
        {
            return new LengthDescriptor(exponent, 0);
        }

        public static LengthDescriptor RadiusLength(int exponent)
        {
            return new LengthDescriptor(0, exponent);
        }
    }
}
