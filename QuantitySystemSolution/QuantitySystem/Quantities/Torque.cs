using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Torque<T> : DerivedQuantity<T>
    {
        public Torque()
            : base(1, new Force<T>(), new Displacement<T>(1))
        {
        }

        public Torque(float exponent)
            : base(exponent, new Force<T>(exponent), new Displacement<T>(exponent))
        {
        }

        /*
        public Torque()
            : base(1, new ForceVector<T>(), new Displacement<T>(1))
        {
        }

        public Torque(float exponent)
            : base(exponent, new ForceVector<T>(exponent), new Displacement<T>(exponent))
        {
        }
        */

        public static implicit operator Torque<T>(T value)
        {
            Torque<T> Q = new Torque<T>();

            Q.Value = value;

            return Q;
        }

    }
}
