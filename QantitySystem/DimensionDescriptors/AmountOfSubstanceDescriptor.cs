namespace QuantitySystem.DimensionDescriptors
{
    public struct AmountOfSubstanceDescriptor : IDimensionDescriptor<AmountOfSubstanceDescriptor>
    {
        public AmountOfSubstanceDescriptor(float exponent):this()
        {
            Exponent = exponent;
        }

        public float Exponent { get; set; }

        public AmountOfSubstanceDescriptor Add(AmountOfSubstanceDescriptor dimensionDescriptor)
        {
            return new AmountOfSubstanceDescriptor
            {
                Exponent = Exponent + dimensionDescriptor.Exponent
            };
        }

        public AmountOfSubstanceDescriptor Subtract(AmountOfSubstanceDescriptor dimensionDescriptor)
        {
            return new AmountOfSubstanceDescriptor
            {
                Exponent = Exponent - dimensionDescriptor.Exponent
            };
        }

        public AmountOfSubstanceDescriptor Multiply(float exponent)
        {
            return new AmountOfSubstanceDescriptor
            {
                Exponent = Exponent * exponent
            };
        }

        public AmountOfSubstanceDescriptor Invert()
        {
            return new AmountOfSubstanceDescriptor
            {
                Exponent = 0 - Exponent
            };
        }
    }
}
