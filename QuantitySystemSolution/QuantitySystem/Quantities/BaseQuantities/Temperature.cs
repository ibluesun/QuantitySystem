using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Temperature<T> : AnyQuantity<T>
    {
        public Temperature() : base(1) { }

        public Temperature(float exponent) : base(exponent) { }

        public override QuantityDimension Dimension
        {
            get
            {
                return new QuantityDimension(0, 0, 0, 1, 0, 0, 0) * Exponent;
            }
        }


        public static implicit operator Temperature<T>(T value)
        {
            Temperature<T> Q = new Temperature<T>();

            Q.Value = value;

            return Q;
        }
    }
}
