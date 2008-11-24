using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities;
using QuantitySystem.Units.Attributes;

namespace QuantitySystem.Units.SI
{
    [SIUnit("J", typeof(Energy<>), SIPrefixes.None)]
    public class Joule : SIUnit
    {
    }
}
