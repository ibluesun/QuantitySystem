using System;
using System.Collections.Generic;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public enum DimensionTensorRank
    {
        /// <summary>
        /// (S) Rank 0 Tensor
        /// </summary>
        Scalar,

        /// <summary>
        /// (V) Rank 1 Tensor
        /// </summary>
        Vector,

        /// <summary>
        /// (M) Rank 2 Tensor
        /// </summary>
        Matrix,

        /// <summary>
        /// ((VM) Tensor of 3rd Rank
        /// </summary>
        VectorMatrix,

        /// <summary>
        /// (MM) Tensor of 4th Rank
        /// </summary>
        MatrixMatrix
    }
}
