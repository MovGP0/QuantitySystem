namespace QuantitySystem.DimensionDescriptors
{
    public struct DigitalDescriptor : IDimensionDescriptor<DigitalDescriptor>
    {
        public DigitalDescriptor(float exponent) : this()
        {
            Exponent = exponent;
        }

        public float Exponent { get; set; }

        public DigitalDescriptor Add(DigitalDescriptor dimensionDescriptor)
        {
            return new DigitalDescriptor
            {
                Exponent = Exponent + dimensionDescriptor.Exponent
            };
        }

        public DigitalDescriptor Subtract(DigitalDescriptor dimensionDescriptor)
        {
            return new DigitalDescriptor
            {
                Exponent = Exponent - dimensionDescriptor.Exponent
            };
        }

        public DigitalDescriptor Multiply(float exponent)
        {
            return new DigitalDescriptor
            {
                Exponent = Exponent * exponent
            };
        }

        public DigitalDescriptor Invert()
        {
            return new DigitalDescriptor
            {
                Exponent = 0 - Exponent
            };
        }
    }
}
