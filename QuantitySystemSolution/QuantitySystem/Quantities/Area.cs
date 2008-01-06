using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Area : DerivedQuantity
    {
        public Area()
            : base(1, new Length(2))
        {
        }

        public Area(int exponent)
            : base(exponent, new Length(2 * exponent))
        {
        }

    }
}
