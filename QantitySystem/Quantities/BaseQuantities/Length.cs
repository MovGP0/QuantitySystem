using QuantitySystem.DimensionDescriptors;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Length<T> : AnyQuantity<T>
    {
        public LengthType LengthType { get; set; }

        public Length() : base(1) 
        {
            LengthType = LengthType.Regular;
        }

        public Length(float exponent) : base(exponent) 
        {
            LengthType = LengthType.Regular;
        }

        public Length(float exponent, LengthType lengthType) : base(exponent)
        {
            LengthType = lengthType;
        }

        public override QuantityDimension Dimension
        {
            get
            {
                var lengthDimension = new QuantityDimension();

                switch (LengthType)
                {
                    case LengthType.Regular:
                        lengthDimension.Length = new LengthDescriptor(Exponent,  0);
                        break;
                    case LengthType.Polar:
                        lengthDimension.Length = new LengthDescriptor(0,  Exponent);
                        break;
                }
                
                return lengthDimension;
            }
        }

        public static implicit operator Length<T>(T value)
        {
            return new Length<T>
            {
                Value = value
            };
        }
    }
}
