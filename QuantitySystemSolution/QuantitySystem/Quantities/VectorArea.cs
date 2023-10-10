using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class VectorArea<T> : DerivedQuantity<T>
    {
        public VectorArea()
            : base(1, new ContraLength<T>(), new CoLength<T>())
        {
        }

        public VectorArea(float exponent)
            : base(exponent, new ContraLength<T>(exponent), new CoLength<T>(exponent))
        {
        }


        public static implicit operator VectorArea<T>(T value)
        {
            VectorArea<T> Q = new VectorArea<T>();

            Q.Value = value;

            return Q;
        }


    }
}
