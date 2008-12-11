using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities;
using QuantitySystem.Units.Attributes;

using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units.SI
{
    [SIUnit("rad", typeof(Angle<>), SIPrefixes.None)]
    public sealed class Radian : SIUnit
    {

    }
}
