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

    [SIUnit("c0", typeof(Velocity<>), SIPrefixes.None)]
    [ReferenceUnit(299792458)]
    public sealed class LightSpeed : SIUnit
    {

    }


    [SIUnit("kn", typeof(Velocity<>), SIPrefixes.None)]
    [ReferenceUnit(1852, 3600)]
    public sealed class Knot : SIUnit
    {

    }

}