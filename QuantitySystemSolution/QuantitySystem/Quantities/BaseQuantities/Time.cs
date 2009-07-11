using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Time<T> : AnyQuantity<T>
    {

        public Time() : base(1) { }

        public Time(float dimension) : base(dimension) { }

        public override QuantityDimension Dimension
        {
            get
            {
                return new QuantityDimension(0, 0, 1) * Exponent;
            }
        }


        public static implicit operator Time<T>(T value)
        {
            Time<T> Q = new Time<T>();

            Q.Value = value;

            return Q;
        }
    }
}
