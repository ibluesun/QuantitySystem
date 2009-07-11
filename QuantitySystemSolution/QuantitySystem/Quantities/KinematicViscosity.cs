using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities
{
    public class KinematicViscosity<T> : DerivedQuantity<T>
    {
        public KinematicViscosity()
            : base(1, new Viscosity<T>(), new Density<T>(-1))
        {
        }

        public KinematicViscosity(float exponent)
            : base(exponent, new Viscosity<T>(exponent), new Density<T>(-1 * exponent))
        {
        }

        public static implicit operator KinematicViscosity<T>(T value)
        {
            KinematicViscosity<T> Q = new KinematicViscosity<T>();

            Q.Value = value;

            return Q;
        }

    }
}
