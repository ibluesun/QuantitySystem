using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    /// <summary>
    /// Dynamic Viscosity.
    /// Pascal Second.
    /// </summary>
    public class Viscosity : DerivedQuantity
    {
        public Viscosity()
            : base(1, new Pressure(), new Time())
        {
        }

        public Viscosity(int exponent)
            : base (exponent, new Pressure(exponent), new Time(exponent))
        {
        }
    }
}
