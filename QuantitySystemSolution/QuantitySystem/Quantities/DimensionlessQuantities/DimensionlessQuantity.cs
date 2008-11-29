using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities.DimensionlessQuantities
{
    public abstract class DimensionlessQuantity<T> : DerivedQuantity<T>
    {

        public DimensionlessQuantity(int exponent, params AnyQuantity<T>[] internalDimensions)
            : base(exponent, internalDimensions)
        {

        }

        



    }
}
