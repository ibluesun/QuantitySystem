using System;
using System.Collections.Generic;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{


    /// <summary>
    /// Displacement as a 2nd rank
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisplacementRank2Tensor<T> : Length<T>
    {
        public DisplacementRank2Tensor() : base(1)
        {
            LengthRank = DimensionTensorRank.Matrix;
        }

        public DisplacementRank2Tensor(float exponent) : base(exponent)
        {
            LengthRank = DimensionTensorRank.Matrix;
        }

    }

    /// <summary>
    /// Displacement as a 2nd rank
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisplacementRank3Tensor<T> : Length<T>
    {
        public DisplacementRank3Tensor() : base(1)
        {
            LengthRank = DimensionTensorRank.VectorMatrix;
        }

        public DisplacementRank3Tensor(float exponent) : base(exponent)
        {
            LengthRank = DimensionTensorRank.VectorMatrix;
        }

    }


    /// <summary>
    /// Displacement as a 2nd rank
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisplacementRank4Tensor<T> : Length<T>
    {
        public DisplacementRank4Tensor() : base(1)
        {
            LengthRank = DimensionTensorRank.MatrixMatrix;
        }

        public DisplacementRank4Tensor(float exponent) : base(exponent)
        {
            LengthRank = DimensionTensorRank.MatrixMatrix;
        }

    }
}
