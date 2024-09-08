using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.DimensionDescriptors;

namespace QuantitySystem.Quantities.BaseQuantities
{



    public class Length<T> : AnyQuantity<T>
    {

        public DimensionTensorRank LengthRank { get; set; }

        public Length() : base(1) 
        {
            LengthRank = DimensionTensorRank.Scalar;
        }

        public Length(float exponent)
            : base(exponent) 
        {
            LengthRank = DimensionTensorRank.Scalar;
        }

        public Length(float exponent, DimensionTensorRank lengthType)
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
                    case DimensionTensorRank.Scalar:
                        LengthDimension.Length = new LengthDescriptor(Exponent,  0, 0, 0, 0);
                        break;
                    case DimensionTensorRank.Vector:
                        LengthDimension.Length = new LengthDescriptor(0, Exponent, 0, 0, 0);
                        break;
                    case DimensionTensorRank.Matrix:
                        LengthDimension.Length = new LengthDescriptor(0, 0, Exponent, 0, 0);
                        break;
                    case DimensionTensorRank.VectorMatrix:
                        LengthDimension.Length = new LengthDescriptor(0, 0, 0, Exponent, 0);
                        break;
                    case DimensionTensorRank.MatrixMatrix:
                        LengthDimension.Length = new LengthDescriptor(0, 0, 0, 0, Exponent);
                        break;
                    default:
                        throw new NotSupportedException("Not Supported Length Descritpor");
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
