using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class StressVector<T> : DerivedQuantity<T>
    {
        public StressVector()
            : base(1, new Force<T>(), new PolarArea<T>(-1))
        {
        }

        public StressVector(float exponent)
            : base(exponent, new Force<T>(exponent), new PolarArea<T>(-1 * exponent))
        {
        }


        public static implicit operator StressVector<T>(T value)
        {
            StressVector<T> Q = new StressVector<T>();

            Q.Value = value;

            return Q;
        }


    }
    public class StressTensor<T> : DerivedQuantity<T>
    {
        public StressTensor()
            : base(1, new Force<T>(), new AreaRank2Tensor<T>(-1))
        {
        }

        public StressTensor(float exponent)
            : base(exponent, new Force<T>(exponent), new AreaRank2Tensor<T>(-1 * exponent))
        {
        }


        public static implicit operator StressTensor<T>(T value)
        {
            StressTensor<T> Q = new StressTensor<T>();

            Q.Value = value;

            return Q;
        }


    }
}
