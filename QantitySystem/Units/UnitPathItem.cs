using System.Globalization;

namespace QuantitySystem.Units
{
    public class UnitPathItem
    {

        public Unit Unit { get; set; }

        public double Times => Numerator / Denominator;

        public double Numerator { get; set; }

        public double Denominator { get; set; }



        /// <summary>
        /// Invert the item 
        /// </summary>
        public void Invert()
        {
            var num = Numerator;
            Numerator = Denominator;
            Denominator = num;
        }


        public override bool Equals(object obj)
        {
            var upi = obj as UnitPathItem;

            if (upi != null)
            {
                if ((Unit.GetType() == upi.Unit.GetType())
                    && (Numerator == upi.Numerator)
                    && (Denominator == upi.Denominator))
                    return true;
                return false;
            }

            return false;
        }

        public override string ToString()
        {

            return Unit.Symbol + ": " + Times.ToString(CultureInfo.InvariantCulture);
        }

    }
}
