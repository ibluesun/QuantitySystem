using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Linq.Expressions;
using System.Globalization;



namespace QuantitySystem.Units
{


    public class UnitPath:Stack<UnitPathItem>, ICloneable
    {

        //I should add to expression tree here.

        public override bool Equals(object obj)
        {
            UnitPath up = obj as UnitPath;

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

        public object Clone()
        {
            UnitPath up = new UnitPath();
            foreach (UnitPathItem upi in this.Reverse())
            {
                up.Push(new UnitPathItem
                {
                    Denumenator = upi.Denumenator,
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
