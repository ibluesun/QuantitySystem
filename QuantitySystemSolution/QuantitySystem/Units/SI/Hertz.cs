using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities;
using QuantitySystem.Units.Attributes;

namespace QuantitySystem.Units.SI
{
    [SIUnit("Hz", typeof(Frequency<>), SIPrefixes.None)]
    public class Hertz : SIUnit
    {
    }
}
