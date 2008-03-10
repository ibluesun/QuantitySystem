using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities;
using QuantitySystem.Units.SIUnits;

using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units
{

    public class Radian : SIUnit<Angle>
    {
        public override string Symbol
        {
            get
            {
                return "rad";
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
