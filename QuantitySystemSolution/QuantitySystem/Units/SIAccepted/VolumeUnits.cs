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

    [SIUnit("L", typeof(Volume<>), SIPrefixes.None)]
    [ReferenceUnit(1, 1000)]  //Litre = 1/1000 m^3
    public sealed class Litre : SIUnit
    {

    }


}