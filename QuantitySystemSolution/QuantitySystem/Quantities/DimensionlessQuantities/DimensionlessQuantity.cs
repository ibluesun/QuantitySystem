using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities.DimensionlessQuantities
{
    public class DimensionlessQuantity : DerivedQuantity
    {

        public DimensionlessQuantity()
            : base(1, new Mass(0), new Length(0), new Time(0))
        {
        }

        public DimensionlessQuantity(int exponent, params AnyQuantity[] internalDimensions)
            : base (exponent, internalDimensions)
        {
            
        }

        



    }
}
