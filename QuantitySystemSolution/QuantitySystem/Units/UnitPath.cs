using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Linq.Expressions;



namespace QuantitySystem.Units
{
    public class UnitPathItem
    {
        //what to discover in the unit
        //Unit 
        // Reference Time


        public Unit Unit { get; set; }

        public double Times
        {
            get
            {
                return Numerator / Denumenator;
            }
        }

        public double Numerator { get; set; }

        public double Denumenator { get; set; }


        public override bool Equals(object obj)
        {
            UnitPathItem upi = obj as UnitPathItem;

            if (upi != null)
            {
                if ((this.Unit.GetType() == upi.Unit.GetType()) 
                    && (this.Numerator == upi.Numerator)
                    && (this.Denumenator == upi.Denumenator))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {

            return Unit.Symbol + ": " + Times.ToString();
        }

    }

    public class UnitPath:Stack<UnitPathItem>
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

    }
}
