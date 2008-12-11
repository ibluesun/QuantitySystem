using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

using QuantitySystem.Units.Attributes;

namespace QuantitySystem.Units.SI
{
    [SIUnit("cd", typeof(LuminousIntensity<>), SIPrefixes.None)]
    public sealed class Candela : SIUnit
    {

    }
}
