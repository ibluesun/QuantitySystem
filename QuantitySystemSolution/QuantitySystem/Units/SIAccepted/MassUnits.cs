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

    [SIUnit("t", typeof(Mass<>), SIPrefixes.None)]
    [ReferenceUnit(1E+3)]
    public sealed class Tonne : SIUnit
    {

    }


    [SIUnit("Da", typeof(Mass<>), SIPrefixes.None)]
    [ReferenceUnit(1.6605388628E-27)]
    public sealed class Dalton : SIUnit
    {

    }


    [SIUnit("me", typeof(Mass<>), SIPrefixes.None)]
    [ReferenceUnit(9.109382616E-31)]
    public sealed class ElectronMass : SIUnit
    {

    }
}