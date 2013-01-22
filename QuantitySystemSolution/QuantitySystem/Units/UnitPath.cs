using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Globalization;

namespace QuantitySystem.Units
{
    public class UnitPathStack:Stack<UnitPathItem>
    {

        public override bool Equals(object obj)
        {
            UnitPathStack up = obj as UnitPathStack;

            if (up != null)
            {
                //compare with count indexing
                if (up.Count == this.Count)
                {
                    for (int ix = 0; ix < Count; ix++)
                    {
                        if (this.ElementAt(ix).Equals(up.ElementAt(ix)) == false)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    //not the same count WRONG.
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public double ConversionFactor
        {
            get
            {

                double cf = 1;
                int ix = 0;
                while (ix < this.Count)
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
            UnitPathStack up = new UnitPathStack();
            foreach (UnitPathItem upi in this.Reverse())
            {
                up.Push(new UnitPathItem
                {
                    Denominator = upi.Denominator,
                    Numerator = upi.Numerator,
                    //Shift = upi.Shift,
                    Unit = (Unit)upi.Unit.Clone()

                });

            }
            return up;
        }

        #endregion
    }
}
