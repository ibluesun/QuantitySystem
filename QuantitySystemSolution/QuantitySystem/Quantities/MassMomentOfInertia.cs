using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class MassMomentOfInertia<T> : DerivedQuantity<T>
    {
        public MassMomentOfInertia()
            : base(1, new Mass<T>(), new RadiusLength<T>(2))
        {
        }

        public MassMomentOfInertia(float exponent)
            : base(exponent, new Mass<T>(exponent), new RadiusLength<T>(2 * exponent))
        {
        }


        public static implicit operator MassMomentOfInertia<T>(T value)
        {
            MassMomentOfInertia<T> Q = new MassMomentOfInertia<T>();

            Q.Value = value;

            return Q;
        }

    }
}
