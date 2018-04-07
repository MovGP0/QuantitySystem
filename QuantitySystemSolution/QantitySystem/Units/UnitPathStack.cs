using System.Collections.Generic;
using System.Linq;

namespace QuantitySystem.Units
{
    public class UnitPathStack: Stack<UnitPathItem>
    {
        public override bool Equals(object obj)
        {
            if (obj is UnitPathStack up)
            {
                //compare with count indexing
                if (up.Count == Count)
                {
                    for (var ix = 0; ix < Count; ix++)
                    {
                        if (this.ElementAt(ix).Equals(up.ElementAt(ix)) == false)
                        {
                            return false;
                        }
                    }
                    return true;
                }

                //not the same count WRONG.
                return false;
            }

            return false;
        }

        public double ConversionFactor
        {
            get
            {

                double cf = 1;
                var ix = 0;
                while (ix < Count)
                {
                    cf = cf * this.ElementAt(ix).Times;
                    ix++;
                }
                return cf;
            }
        }

        #region ICloneable Members

        public UnitPathStack Clone()
        {
            var up = new UnitPathStack();
            foreach (var upi in this.Reverse())
            {
                up.Push(new UnitPathItem
                {
                    Denominator = upi.Denominator,
                    Numerator = upi.Numerator,
                    Unit = upi.Unit.Clone()
                });
            }
            return up;
        }
        #endregion
    }
}
