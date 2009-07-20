using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace QuantitySystem.Units
{
    public class UnitPathItem
    {

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



        /// <summary>
        /// Invert the item 
        /// </summary>
        public void Invert()
        {
            double num = Numerator;
            Numerator = Denumenator;
            Denumenator = num;
        }


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

            return Unit.Symbol + ": " + Times.ToString(CultureInfo.InvariantCulture);
        }

    }
}
