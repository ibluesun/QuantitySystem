using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Volume : DerivedQuantity
    {
        public Volume()
            : base(1, new Length(3))
        {
        }

        public Volume(int exponent)
            : base(exponent, new Length(3 * exponent))
        {
        }

    }
}
