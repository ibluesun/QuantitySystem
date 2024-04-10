using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.DimensionDescriptors;

namespace QuantitySystem.Quantities.BaseQuantities
{

    public enum TensorRank
    {
        /// <summary>
        /// Rank 0
        /// </summary>
        Scalar,

        /// <summary>
        /// Rank 1
        /// </summary>
        Vector,

        /// <summary>
        /// Rank 2
        /// </summary>
        Matrix,

        /// <summary>
        /// Rank 3
        /// </summary>
        MV,

        /// <summary>
        /// Rank 4
        /// </summary>
        MM
    }

    public class Length<T> : AnyQuantity<T>
    {

        public TensorRank LengthRank { get; set; }

        public Length() : base(1) 
        {
            LengthRank = TensorRank.Scalar;
        }

        public Length(float exponent)
            : base(exponent) 
        {
            LengthRank = TensorRank.Scalar;
        }

        public Length(float exponent, TensorRank lengthType)
            : base(exponent)
        {
            LengthRank = lengthType;
        }

        public override QuantityDimension Dimension
        {
            get
            {
                QuantityDimension LengthDimension = new QuantityDimension();

                switch (LengthRank)
                {
                    case TensorRank.Scalar:
                        LengthDimension.Length = new LengthDescriptor(Exponent,  0);
                        break;
                    case TensorRank.Vector:
                        LengthDimension.Length = new LengthDescriptor(0,  Exponent);
                        break;
                }
                
                return LengthDimension;
            }
        }


        public static implicit operator Length<T>(T value)
        {
            Length<T> Q = new Length<T>();

            Q.Value = value;

            return Q;
        }

    }

}
