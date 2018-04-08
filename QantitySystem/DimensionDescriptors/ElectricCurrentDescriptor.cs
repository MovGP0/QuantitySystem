namespace QuantitySystem.DimensionDescriptors
{
    public struct ElectricCurrentDescriptor : IDimensionDescriptor<ElectricCurrentDescriptor>
    {
        public ElectricCurrentDescriptor(float exponent):this()
        {
            Exponent = exponent;
        }

        public float Exponent { get; set; }

        public ElectricCurrentDescriptor Add(ElectricCurrentDescriptor dimensionDescriptor)
        {
            return new ElectricCurrentDescriptor
            {
                Exponent = Exponent + dimensionDescriptor.Exponent
            };
        }

        public ElectricCurrentDescriptor Subtract(ElectricCurrentDescriptor dimensionDescriptor)
        {
            return new ElectricCurrentDescriptor
            {
                Exponent = Exponent - dimensionDescriptor.Exponent
            };
        }

        public ElectricCurrentDescriptor Multiply(float exponent)
        {
            return new ElectricCurrentDescriptor
            {
                Exponent = Exponent * exponent
            };
        }

        public ElectricCurrentDescriptor Invert()
        {
            return new ElectricCurrentDescriptor
            {
                Exponent = 0 - Exponent
            };
        }
    }
}
