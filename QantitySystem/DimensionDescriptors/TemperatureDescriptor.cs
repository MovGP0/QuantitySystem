namespace QuantitySystem.DimensionDescriptors
{
    public struct TemperatureDescriptor : IDimensionDescriptor<TemperatureDescriptor>
    {
        public TemperatureDescriptor(float exponent):this()
        {
            Exponent = exponent;
        }
        
        public float Exponent { get; set; }

        public TemperatureDescriptor Add(TemperatureDescriptor dimensionDescriptor)
        {
            return new TemperatureDescriptor
            {
                Exponent = Exponent + dimensionDescriptor.Exponent
            };
        }

        public TemperatureDescriptor Subtract(TemperatureDescriptor dimensionDescriptor)
        {
            return new TemperatureDescriptor
            {
                Exponent = Exponent - dimensionDescriptor.Exponent
            };
        }

        public TemperatureDescriptor Multiply(float exponent)
        {
            return new TemperatureDescriptor
            {
                Exponent = Exponent * exponent
            };
        }

        public TemperatureDescriptor Invert()
        {
            return new TemperatureDescriptor
            {
                Exponent = 0 - Exponent
            };
        }
    }
}
