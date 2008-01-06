using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities
{
    public class KinematicViscosity : DerivedQuantity
    {
        public KinematicViscosity()
            : base(1, new Viscosity(), new Density(-1))
        {
        }

        public KinematicViscosity(int exponent)
            : base(exponent, new Viscosity(exponent), new Density(-1 * exponent))
        {
        }
    }
}
