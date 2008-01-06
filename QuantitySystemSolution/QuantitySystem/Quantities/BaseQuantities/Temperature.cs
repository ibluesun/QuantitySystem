using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Temperature : AnyQuantity
    {
        public Temperature() : base(1) { }

        public Temperature(int exponent) : base(exponent) { }

        public override QuantityDimension Dimension
        {
            get
            {
                return new QuantityDimension(0, 0, 0, 1, 0, 0, 0) * Exponent;
            }
        }

    }
}
