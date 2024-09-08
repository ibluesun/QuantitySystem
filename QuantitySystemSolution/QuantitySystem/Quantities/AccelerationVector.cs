using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class AccelerationVector<T> : DerivedQuantity<T>
    {
        public AccelerationVector()
            : base(1, new Velocity<T>(), new Time<T>(-1))
        {
        }

        public AccelerationVector(float exponent)
            : base(exponent, new Velocity<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator AccelerationVector<T>(T value)
        {
            AccelerationVector<T> Q = new AccelerationVector<T>();

            Q.Value = value;

            return Q;
        }


    }


    public class AccelerationRank2Tensor<T> : DerivedQuantity<T>
    {
        public AccelerationRank2Tensor()
            : base(1, new VelocityRank2Tensor<T>(), new Time<T>(-1))
        {
        }

        public AccelerationRank2Tensor(float exponent)
            : base(exponent, new VelocityRank2Tensor<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator AccelerationRank2Tensor<T>(T value)
        {
            AccelerationRank2Tensor<T> Q = new AccelerationRank2Tensor<T>();

            Q.Value = value;

            return Q;
        }


    }


    public class AccelerationRank3Tensor<T> : DerivedQuantity<T>
    {
        public AccelerationRank3Tensor()
            : base(1, new VelocityRank3Tensor<T>(), new Time<T>(-1))
        {
        }

        public AccelerationRank3Tensor(float exponent)
            : base(exponent, new VelocityRank3Tensor<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator AccelerationRank3Tensor<T>(T value)
        {
            AccelerationRank3Tensor<T> Q = new AccelerationRank3Tensor<T>();

            Q.Value = value;

            return Q;
        }


    }

    public class AccelerationRank4Tensor<T> : DerivedQuantity<T>
    {
        public AccelerationRank4Tensor()
            : base(1, new VelocityRank4Tensor<T>(), new Time<T>(-1))
        {
        }

        public AccelerationRank4Tensor(float exponent)
            : base(exponent, new VelocityRank4Tensor<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator AccelerationRank4Tensor<T>(T value)
        {
            AccelerationRank4Tensor<T> Q = new AccelerationRank4Tensor<T>();

            Q.Value = value;

            return Q;
        }


    }

}
