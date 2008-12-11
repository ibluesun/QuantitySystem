
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;


using QuantitySystem.Units.Attributes;
using QuantitySystem.Quantities;
using QuantitySystem.Units.SI;


namespace QuantitySystem.Units.SIAccepted
{

    [SIUnit("bar", typeof(Pressure<>), SIPrefixes.None)]
    [ReferenceUnit(1E+5)]
    public sealed class Bar : SIUnit
    {

    }

}