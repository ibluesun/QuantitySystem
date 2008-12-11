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

    [SIUnit("ha", typeof(Area<>), SIPrefixes.None)]
    [ReferenceUnit(1E+4)]
    public sealed class Hectare : SIUnit
    {

    }

    [SIUnit("b", typeof(Area<>), SIPrefixes.None)]
    [ReferenceUnit(1E-28)]
    public sealed class Barn : SIUnit
    {

    }


}