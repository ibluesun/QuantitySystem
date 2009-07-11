using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class LuminousIntensity<T> : AnyQuantity<T>
    {
        public LuminousIntensity() : base(1) { }

        public LuminousIntensity(float exponent) : base(exponent) { }

        public override QuantityDimension Dimension
        {
            get
            {
                return new QuantityDimension(0, 0, 0, 0, 0, 0, 1) * Exponent;
            }
        }


        public static implicit operator LuminousIntensity<T>(T value)
        {
            LuminousIntensity<T> Q = new LuminousIntensity<T>();

            Q.Value = value;

            return Q;
        }

    }
}
