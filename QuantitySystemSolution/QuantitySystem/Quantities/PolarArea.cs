using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class PolarArea<T> : DerivedQuantity<T>
    {
        public PolarArea()
            : base(1, new PolarLength<T>(2))
        {
        }

        public PolarArea(float exponent)
            : base(exponent, new PolarLength<T>(2 * exponent))
        {
        }


        public static implicit operator PolarArea<T>(T value)
        {
            PolarArea<T> Q = new PolarArea<T>();

            Q.Value = value;

            return Q;
        }


    }
}
