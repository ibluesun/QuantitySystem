using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units.UnitSystems;

namespace QuantitySystem.Units.SIUnits
{
    public class Newton : SIUnit<Force>
    {


        public override string Symbol
        {
            get
            {
                return base.Symbol + "N";
            }
        }




        public override ISIUnit GetUnitInBaseUnits()
        {
            BaseUnits.Gram kg = new BaseUnits.Gram();
            kg.Prefix += Prefix;
            
            ISIUnit m = new BaseUnits.Metre();

            ISIUnit s = new Second();
            s.Exponent = -2;

            DerivedSIUnit dsi = new DerivedSIUnit(kg, m, s);

            if (Exponent < 0) dsi = (DerivedSIUnit)dsi.Invert(); //invert the units because it may be inverted from the begining.


            return dsi;

            


        }

    }
}
