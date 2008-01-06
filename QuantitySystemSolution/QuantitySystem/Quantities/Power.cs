using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Power : DerivedQuantity
    {
        public Power()
            : base(1, new Energy(), new Time(-1))
        {
        }

        public Power(int exponent)
            : base(exponent, new Energy(exponent), new Time(-1 * exponent))
        {
        }
    }
}
