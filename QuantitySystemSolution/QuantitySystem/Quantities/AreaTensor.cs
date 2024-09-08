using QuantitySystem.Quantities.BaseQuantities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuantitySystem.Quantities
{
    public class AreaRank2Tensor<T> : DerivedQuantity<T>
    {
        public AreaRank2Tensor()
            : base(1, new DisplacementRank2Tensor<T>(2))
        {
        }

        public AreaRank2Tensor(float exponent)
            : base(exponent, new DisplacementRank2Tensor<T>(2 * exponent))
        {
        }


        public static implicit operator AreaRank2Tensor<T>(T value)
        {
            AreaRank2Tensor<T> Q = new AreaRank2Tensor<T>();

            Q.Value = value;

            return Q;
        }


    }


    public class AreaRank3Tensor<T> : DerivedQuantity<T>
    {
        public AreaRank3Tensor()
            : base(1, new DisplacementRank3Tensor<T>(2))
        {
        }

        public AreaRank3Tensor(float exponent)
            : base(exponent, new DisplacementRank3Tensor<T>(2 * exponent))
        {
        }


        public static implicit operator AreaRank3Tensor<T>(T value)
        {
            AreaRank3Tensor<T> Q = new AreaRank3Tensor<T>();

            Q.Value = value;

            return Q;
        }


    }
}
