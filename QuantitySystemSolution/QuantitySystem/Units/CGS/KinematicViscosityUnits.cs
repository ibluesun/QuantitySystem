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
    [SIUnit("St", typeof(KinematicViscosity<>), SIPrefixes.None)]
    [ReferenceUnit(1e-4)]
    public sealed class Stokes : SIUnit
    {

    }

}