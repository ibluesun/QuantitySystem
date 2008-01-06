using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.SIUnits
{
    public class Joule : SIUnit<Energy>
    {
        public override string Symbol
        {
            get
            {
                return base.Symbol + "J";
            }
        }


    }
}
