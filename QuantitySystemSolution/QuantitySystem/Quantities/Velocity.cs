using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Velocity : DerivedQuantity
    {
        public Velocity()
            : base(1, new Length(), new Time(-1))
        {
        }

        public Velocity(int exponent)
            : base(exponent, new Length(exponent), new Time(-1 * exponent))
        {
        }
    }
}
