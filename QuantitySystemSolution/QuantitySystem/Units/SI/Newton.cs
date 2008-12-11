using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities;
using QuantitySystem.Units.Attributes;

namespace QuantitySystem.Units.SI
{

    [SIUnit("N", typeof(Force<>), SIPrefixes.None)]
    public sealed class Newton : SIUnit
    {




        //public override SIUnit GetUnitInBaseUnits()
        //{
        //    BaseUnits.Gram kg = new BaseUnits.Gram();
        //    kg.Prefix += Prefix;
            
        //    SIUnit m = new BaseUnits.Metre();

        //    SIUnit s = new Second();
        //    s.Exponent = -2;

        //    DerivedSIUnit dsi = new DerivedSIUnit(kg, m, s);

        //    if (Exponent < 0) dsi = (DerivedSIUnit)dsi.Invert(); //invert the units because it may be inverted from the begining.


        //    return dsi;

        //}

    }
}
