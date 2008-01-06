using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Density : DerivedQuantity
    {

        public Density()
            : base(1, new Mass(), new Volume(-1))
        {
        }

        public Density(int exponent)
            : base(exponent, new Mass(exponent), new Volume(-1 * exponent))
        {
        }

    }
}
