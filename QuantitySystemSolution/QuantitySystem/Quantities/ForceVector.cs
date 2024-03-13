using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;


namespace QuantitySystem.Quantities
{
    public class ForceVector<T> : DerivedQuantity<T>
    {
        public ForceVector()
            : base(1, new Mass<T>(), new AccelerationVector<T>())
        {
        }

        public ForceVector(float exponent)
            : base(exponent, new Mass<T>(exponent), new AccelerationVector<T>(exponent))
        {
        }

        public static implicit operator ForceVector<T>(T value)
        {
            ForceVector<T> Q = new ForceVector<T>();

            Q.Value = value;

            return Q;
        }

    }
}
