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
    [SIUnit("erg", typeof(Energy<>), SIPrefixes.None)]
    [ReferenceUnit(1E-7)]
    public sealed class Erg : SIUnit
    {

    }

}