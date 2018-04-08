namespace QuantitySystem.DimensionDescriptors
{
    public struct LuminousIntensityDescriptor : IDimensionDescriptor<LuminousIntensityDescriptor>
    {
        public LuminousIntensityDescriptor(float exponent) : this()
        {
            Exponent = exponent;
        }

        public float Exponent { get; set; }

        public LuminousIntensityDescriptor Add(LuminousIntensityDescriptor dimensionDescriptor)
        {
            return new LuminousIntensityDescriptor
            {
                Exponent = Exponent + dimensionDescriptor.Exponent
            };
        }

        public LuminousIntensityDescriptor Subtract(LuminousIntensityDescriptor dimensionDescriptor)
        {
            return new LuminousIntensityDescriptor
            {
                Exponent = Exponent - dimensionDescriptor.Exponent
            };
        }

        public LuminousIntensityDescriptor Multiply(float exponent)
        {
            return new LuminousIntensityDescriptor
            {
                Exponent = Exponent * exponent
            };
        }

        public LuminousIntensityDescriptor Invert()
        {
            return new LuminousIntensityDescriptor
            {
                Exponent = 0 - Exponent
            };
        }
    }
}
