using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Quantities
{
    public class AngularVelocity<T> : DerivedQuantity<T>
    {
        public AngularVelocity()
            : base(1, new Angle<T>(), new Time<T>(-1))
        {
        }


        public AngularVelocity(float exponent)
            : base(exponent, new Angle<T>(exponent), new Time<T>(-1 * exponent))
        {
        }

        public static implicit operator AngularVelocity<T>(T value)
        {
            AngularVelocity<T> Q = new AngularVelocity<T>();

            Q.Value = value;

            return Q;
        }

    }
}
