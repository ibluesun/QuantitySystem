using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Pressure : DerivedQuantity
    {
        public Pressure()
            : base(1, new Force(), new Area(-1))
        {
        }

        public Pressure(int exponent)
            : base(exponent, new Force(exponent), new Area(-1 * exponent))
        {
        }

    }
}
