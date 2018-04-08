namespace QuantitySystem.DimensionDescriptors
{
    public struct MassDescriptor : IDimensionDescriptor<MassDescriptor>
    {
        public MassDescriptor(float exponent):this()
        {
            Exponent = exponent;
        }

        public float Exponent { get; set; }

        public MassDescriptor Add(MassDescriptor dimensionDescriptor)
        {
            return new MassDescriptor
            {
                Exponent = Exponent + dimensionDescriptor.Exponent
            };
        }

        public MassDescriptor Subtract(MassDescriptor dimensionDescriptor)
        {
            return new MassDescriptor
            {
                Exponent = Exponent - dimensionDescriptor.Exponent
            };
        }

        public MassDescriptor Multiply(float exponent)
        {
            return new MassDescriptor
            {
                Exponent = Exponent * exponent
            };
        }

        public MassDescriptor Invert()
        {
            return new MassDescriptor
            {
                Exponent = 0 - Exponent
            };
        }
    }
}
