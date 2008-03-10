using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.DimensionDescriptors;

namespace QuantitySystem.Quantities.DimensionlessQuantities
{
    public class Angle : DerivedQuantity
    {

        public Angle()
            : base(1, new Length(1, LengthType.Normal), new Length(-1, LengthType.Radius))
        {
        }

        public Angle(int exponent)
            : base(exponent, new Length(1, LengthType.Normal), new Length(-1, LengthType.Radius))
        {
        }

    }
}
