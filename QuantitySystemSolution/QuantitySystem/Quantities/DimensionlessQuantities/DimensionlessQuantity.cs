using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities.DimensionlessQuantities
{
    public class DimensionlessQuantity<T> : DerivedQuantity<T>
    {

        public DimensionlessQuantity()
            : base(1, new Mass<T>(0), new Length<T>(0), new Time<T>(0))
        {
        }

        public DimensionlessQuantity(int exponent, params AnyQuantity<T>[] internalDimensions)
            : base (exponent, internalDimensions)
        {
            
        }

        



    }
}
