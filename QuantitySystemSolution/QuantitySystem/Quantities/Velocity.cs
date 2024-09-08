using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Velocity<T> : DerivedQuantity<T>
    {
        public Velocity()
            : base(1, new Displacement<T>(), new Time<T>(-1))
        {
        }

        public Velocity(float exponent)
            : base(exponent, new Displacement<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator Velocity<T>(T value)
        {
            Velocity<T> Q = new Velocity<T>();

            Q.Value = value;

            return Q;
        }

    }

    public class VelocityRank2Tensor<T> : DerivedQuantity<T>
    {
        public VelocityRank2Tensor()
            : base(1, new DisplacementRank2Tensor<T>(), new Time<T>(-1))
        {
        }

        public VelocityRank2Tensor(float exponent)
            : base(exponent, new DisplacementRank2Tensor<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator VelocityRank2Tensor<T>(T value)
        {
            VelocityRank2Tensor<T> Q = new VelocityRank2Tensor<T>();

            Q.Value = value;

            return Q;
        }

    }

    public class VelocityRank3Tensor<T> : DerivedQuantity<T>
    {
        public VelocityRank3Tensor()
            : base(1, new DisplacementRank3Tensor<T>(), new Time<T>(-1))
        {
        }

        public VelocityRank3Tensor(float exponent)
            : base(exponent, new DisplacementRank3Tensor<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator VelocityRank3Tensor<T>(T value)
        {
            VelocityRank3Tensor<T> Q = new VelocityRank3Tensor<T>();

            Q.Value = value;

            return Q;
        }

    }

    public class VelocityRank4Tensor<T> : DerivedQuantity<T>
    {
        public VelocityRank4Tensor()
            : base(1, new DisplacementRank4Tensor<T>(), new Time<T>(-1))
        {
        }

        public VelocityRank4Tensor(float exponent)
            : base(exponent, new DisplacementRank4Tensor<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator VelocityRank4Tensor<T>(T value)
        {
            VelocityRank4Tensor<T> Q = new VelocityRank4Tensor<T>();

            Q.Value = value;

            return Q;
        }

    }
}
