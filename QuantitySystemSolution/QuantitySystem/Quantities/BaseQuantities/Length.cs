using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Length : AnyQuantity
    {

        public Length() : base(1) { }

        public Length(int exponent) : base(exponent) { }


        public override QuantityDimension Dimension
        {
            get
            {
                return new QuantityDimension(0, 1, 0) * Exponent;
            }
        }
    }
}
