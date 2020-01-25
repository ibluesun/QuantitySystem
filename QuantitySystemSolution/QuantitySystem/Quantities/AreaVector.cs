using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class AreaVector<T> : DerivedQuantity<T>
    {
        public AreaVector()
            : base(1, new LengthVector<T>(2))
        {
        }

        public AreaVector(float exponent)
            : base(exponent, new LengthVector<T>(2 * exponent))
        {
        }


        public static implicit operator AreaVector<T>(T value)
        {
            AreaVector<T> Q = new AreaVector<T>();

            Q.Value = value;

            return Q;
        }


    }
}
