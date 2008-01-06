using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units.SIUnits;

namespace QuantitySystem.Units
{
    public class Second : SIUnit<Time>
    {


        public override string Symbol
        {
            get
            {
                return base.Symbol + "s";
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
