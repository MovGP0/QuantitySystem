namespace QuantitySystem.DimensionDescriptors
{
    public struct TimeDescriptor : IDimensionDescriptor<TimeDescriptor>
    {
        public TimeDescriptor(float exponent):this()
        {
            Exponent = exponent;
        }
        
        public float Exponent { get; set; }

        public TimeDescriptor Add(TimeDescriptor dimensionDescriptor)
        {
            return new TimeDescriptor
            {
                Exponent = Exponent + dimensionDescriptor.Exponent
            };
        }

        public TimeDescriptor Subtract(TimeDescriptor dimensionDescriptor)
        {
            return new TimeDescriptor
            {
                Exponent = Exponent - dimensionDescriptor.Exponent
            };
        }

        public TimeDescriptor Multiply(float exponent)
        {
            return new TimeDescriptor
            {
                Exponent = Exponent * exponent
            };
        }

        public TimeDescriptor Invert()
        {
            return new TimeDescriptor
            {
                Exponent = 0 - Exponent
            };
        }
    }
}
