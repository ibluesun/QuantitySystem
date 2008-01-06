using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class ElectricalCurrent : AnyQuantity
    {
        public ElectricalCurrent() : base(1) { }

        public ElectricalCurrent(int exponent) : base(exponent) { }

        public override QuantityDimension Dimension
        {
            get
            {
                return new QuantityDimension(0, 0, 0, 0, 1, 0, 0) * Exponent;
            }
        }

    }
}
