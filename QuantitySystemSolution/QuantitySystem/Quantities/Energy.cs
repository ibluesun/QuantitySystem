using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Energy : DerivedQuantity
    {
        public Energy()
            : base(1, new Force(), new Length())
        {
        }

        public Energy(int exponent)
            : base(exponent, new Force(exponent), new Length(exponent))
        {
        }

    }
}
