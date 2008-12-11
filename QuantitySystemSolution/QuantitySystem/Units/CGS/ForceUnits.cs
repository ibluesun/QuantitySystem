using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;


using QuantitySystem.Units.Attributes;
using QuantitySystem.Quantities;
using QuantitySystem.Units.SI;


namespace QuantitySystem.Units.CGS
{
    [SIUnit("dyn", typeof(Force<>), SIPrefixes.None)]
    [ReferenceUnit(1e-5)]
    public sealed class Dyne : SIUnit
    {

    }

}