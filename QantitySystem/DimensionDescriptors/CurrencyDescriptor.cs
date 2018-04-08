namespace QuantitySystem.DimensionDescriptors
{
    public struct CurrencyDescriptor : IDimensionDescriptor<CurrencyDescriptor>
    {
        public CurrencyDescriptor(float exponent):this()
        {
            Exponent = exponent;
        }
        
        public float Exponent { get; set; }

        public CurrencyDescriptor Add(CurrencyDescriptor dimensionDescriptor)
        {
            return new CurrencyDescriptor
            {
                Exponent = Exponent + dimensionDescriptor.Exponent
            };
        }

        public CurrencyDescriptor Subtract(CurrencyDescriptor dimensionDescriptor)
        {
            return new CurrencyDescriptor
            {
                Exponent = Exponent - dimensionDescriptor.Exponent
            };
        }

        public CurrencyDescriptor Multiply(float exponent)
        {
            return new CurrencyDescriptor
            {
                Exponent = Exponent * exponent
            };
        }

        public CurrencyDescriptor Invert()
        {
            return new CurrencyDescriptor
            {
                Exponent = 0 - Exponent
            };
        }
    }
}
