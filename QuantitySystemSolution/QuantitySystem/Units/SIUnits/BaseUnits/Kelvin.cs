using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.SIUnits.BaseUnits
{
    public class Kelvin : SIUnit<Temperature>
    {

        public override string Symbol
        {
            get
            {
                return base.Symbol + "K";
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
