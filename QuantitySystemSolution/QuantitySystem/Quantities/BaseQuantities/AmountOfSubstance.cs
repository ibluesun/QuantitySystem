using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class AmountOfSubstance : AnyQuantity
    {
        public AmountOfSubstance() : base(1) { }

        public AmountOfSubstance(int exponent) : base(exponent) { }

        public override QuantityDimension Dimension
        {
            get
            {
                return new QuantityDimension(0, 0, 0, 0, 0, 1, 0) * Exponent;
            }
        }
    }
}
