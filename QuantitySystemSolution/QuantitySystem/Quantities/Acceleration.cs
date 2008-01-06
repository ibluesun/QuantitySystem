using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Acceleration : DerivedQuantity
    {
        public Acceleration()
            : base(1, new Velocity(), new Time(-1))
        {
        }

        public Acceleration(int exponent)
            : base(exponent, new Velocity(exponent), new Time(-1 * exponent))
        {
        }

    }
}
