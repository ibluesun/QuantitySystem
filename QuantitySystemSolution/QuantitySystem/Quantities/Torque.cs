using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Torque : DerivedQuantity
    {
        public Torque()
            : base(1, new Force(), new Length(1, LengthType.Radius))
        {
            
        }
    }
}
