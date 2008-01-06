using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.SIUnits.BaseUnits
{
    public class Candela : SIUnit<LuminousIntensity>
    {




        public override string Symbol
        {
            get
            {
                return base.Symbol + "cd";
            }
        }

        public override bool IsBaseUnit
        {
            get
            {
                return true;
            }
        }

    }
}
