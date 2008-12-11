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

    [SIUnit("ua", typeof(Length<>), SIPrefixes.None)]
    [ReferenceUnit(1.495978706916E+11)]
    public sealed class AstronomicalUnit : SIUnit
    {

    }

    [SIUnit("Å", typeof(Length<>), SIPrefixes.None)]
    [ReferenceUnit(1E-10)]
    public sealed class Angstrom : SIUnit
    {

    }

    [SIUnit("M", typeof(Length<>), SIPrefixes.None)]
    [ReferenceUnit(1852)]
    public sealed class NauticalMile : SIUnit
    {

    }

}