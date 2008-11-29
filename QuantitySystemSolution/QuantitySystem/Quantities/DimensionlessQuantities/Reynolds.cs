using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities.DimensionlessQuantities
{
    public sealed class Reynolds<T> : DimensionlessQuantity<T>
    {
        public Reynolds()
            :base (1, new Density<T>(), new Velocity<T>(), new Length<T>(), new Viscosity<T>(-1))
        {
        }

        public static implicit operator Reynolds<T>(T value)
        {
            Reynolds<T> Q = new Reynolds<T>();

            Q.Value = value;

            return Q;
        }

    }
}
