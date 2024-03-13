using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class AccelerationVector<T> : DerivedQuantity<T>
    {
        public AccelerationVector()
            : base(1, new Velocity<T>(), new Time<T>(-1))
        {
        }

        public AccelerationVector(float exponent)
            : base(exponent, new Velocity<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator AccelerationVector<T>(T value)
        {
            AccelerationVector<T> Q = new AccelerationVector<T>();

            Q.Value = value;

            return Q;
        }


    }
}
