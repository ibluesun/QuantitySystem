using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.SIUnits.BaseUnits
{
    public class Mole : SIUnit<AmountOfSubstance>
    {



        public override string Symbol
        {
            get
            {
                return base.Symbol + "mol";
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
