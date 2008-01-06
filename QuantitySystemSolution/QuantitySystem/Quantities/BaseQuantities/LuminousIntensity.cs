using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class LuminousIntensity : AnyQuantity
    {
        public LuminousIntensity() : base(1) { }

        public LuminousIntensity(int exponent) : base(exponent) { }

        public override QuantityDimension Dimension
        {
            get
            {
                return new QuantityDimension(0, 0, 0, 0, 0, 0, 1) * Exponent;
            }
        }
    }
}
