
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
    [SIUnit("eV", typeof(Energy<>), SIPrefixes.None)]
    [ReferenceUnit(1.6021765314E-19)]
    public sealed class ElectronVolt : SIUnit
    {

    }

}