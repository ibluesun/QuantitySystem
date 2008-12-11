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
    [SIUnit("P", typeof(Viscosity<>), SIPrefixes.None)]
    [ReferenceUnit(0.1)]
    public sealed class Poise : SIUnit
    {

    }

}