using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities.DimensionlessQuantities
{
    public class Reynolds : DimensionlessQuantity
    {
        public Reynolds()
            :base (1, new Density(), new Velocity(), new Length(), new Viscosity(-1))
        {
        }
    }
}
