using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;


namespace QuantitySystem.Quantities
{
    public class ForceRank2Tensor<T> : DerivedQuantity<T>
    {
        public ForceRank2Tensor()
            : base(1, new Mass<T>(), new AccelerationRank2Tensor<T>())
        {
        }

        public ForceRank2Tensor(float exponent)
            : base(exponent, new Mass<T>(exponent), new AccelerationRank2Tensor<T>(exponent))
        {
        }

        public static implicit operator ForceRank2Tensor<T>(T value)
        {
            ForceRank2Tensor<T> Q = new ForceRank2Tensor<T>();

            Q.Value = value;

            return Q;
        }

    }
    public class ForceRank3Tensor<T> : DerivedQuantity<T>
    {
        public ForceRank3Tensor()
            : base(1, new Mass<T>(), new AccelerationRank3Tensor<T>())
        {
        }

        public ForceRank3Tensor(float exponent)
            : base(exponent, new Mass<T>(exponent), new AccelerationRank3Tensor<T>(exponent))
        {
        }

        public static implicit operator ForceRank3Tensor<T>(T value)
        {
            ForceRank3Tensor<T> Q = new ForceRank3Tensor<T>();

            Q.Value = value;

            return Q;
        }

    }
    public class ForceRank4Tensor<T> : DerivedQuantity<T>
    {
        public ForceRank4Tensor()
            : base(1, new Mass<T>(), new AccelerationRank4Tensor<T>())
        {
        }

        public ForceRank4Tensor(float exponent)
            : base(exponent, new Mass<T>(exponent), new AccelerationRank4Tensor<T>(exponent))
        {
        }

        public static implicit operator ForceRank4Tensor<T>(T value)
        {
            ForceRank4Tensor<T> Q = new ForceRank4Tensor<T>();

            Q.Value = value;

            return Q;
        }

    }
}
