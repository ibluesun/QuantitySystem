using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Digital<T> : AnyQuantity<T>
    {

        public Digital() : base(1) { }

        public Digital(float exponent) : base(exponent) { }

        private static QuantityDimension _Dimension = new QuantityDimension()
        {
            Digital = new DimensionDescriptors.DigitalDescriptor(1)
        };

        public override QuantityDimension Dimension
        {
            get
            {
                return  _Dimension * Exponent;
            }
        }


        public static implicit operator Digital<T>(T value)
        {
            Digital<T> Q = new Digital<T>();

            Q.Value = value;

            return Q;
        }
    }
}
