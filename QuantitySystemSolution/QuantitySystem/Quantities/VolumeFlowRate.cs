using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class VolumeFlowRate<T> : DerivedQuantity<T>
    {
        public VolumeFlowRate()
            : base(1, new Volume<T>(), new Time<T>(-1))
        {
        }

        public VolumeFlowRate(float exponent)
            : base(exponent, new Volume<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator VolumeFlowRate<T>(T value)
        {
            VolumeFlowRate<T> Q = new VolumeFlowRate<T>();

            Q.Value = value;

            return Q;
        }

    }
}
