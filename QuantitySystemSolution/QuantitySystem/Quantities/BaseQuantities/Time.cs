using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Time : AnyQuantity
    {

        public Time() : base(1) { }

        public Time(int dimension) : base(dimension) { }

        public override QuantityDimension Dimension
        {
            get
            {
                return new QuantityDimension(0, 0, 1) * Exponent;
            }
        }
    }
}
