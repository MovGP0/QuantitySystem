namespace QuantitySystem.Tests
{
    /// <summary>
    /// Complex Number Struct used as a sample for testing 
    /// the container value of the quantity.
    /// </summary>
    public struct ComplexNumber
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public override string ToString()
        {
            return Real + " + " + Imaginary + "i";
        }

        public static ComplexNumber operator +(ComplexNumber left, ComplexNumber right)
        {
            return new ComplexNumber
            {
                Real = left.Real + right.Real,
                Imaginary = left.Imaginary + right.Imaginary
            };
        }

        public static ComplexNumber operator +(ComplexNumber left, int right)
        {
            return new ComplexNumber
            {
                Real = left.Real + right,
                Imaginary = left.Imaginary
            };
        }

        public static ComplexNumber operator -(ComplexNumber left, ComplexNumber right)
        {
            return new ComplexNumber
            {
                Real = left.Real - right.Real,
                Imaginary = left.Imaginary - right.Imaginary
            };
        }

        public static ComplexNumber operator *(ComplexNumber left, ComplexNumber right)
        {
            return new ComplexNumber
            {
                Real = left.Real * right.Real,
                Imaginary = left.Imaginary * right.Imaginary
            };
        }

        public static ComplexNumber operator /(ComplexNumber left, ComplexNumber right)
        {
            return new ComplexNumber
            {
                Real = left.Real / right.Real,
                Imaginary = left.Imaginary / right.Imaginary
            };
        }

        public static ComplexNumber operator /(double f, ComplexNumber c)
        {
            return new ComplexNumber
            {
                Real = f / c.Real,
                Imaginary = f / c.Imaginary
            };
        }

        public override bool Equals(object obj)
        {
            if (GetType() == obj.GetType())
            {
                var cn = (ComplexNumber)obj;

                return cn.Imaginary == Imaginary
                    && cn.Real == Real;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -837395861;
            hashCode = hashCode * -1521134295 + Real.GetHashCode();
            hashCode = hashCode * -1521134295 + Imaginary.GetHashCode();
            return hashCode;
        }
    }
}
