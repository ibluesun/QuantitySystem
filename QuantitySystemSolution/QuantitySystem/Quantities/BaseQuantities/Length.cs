using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.DimensionDescriptors;

namespace QuantitySystem.Quantities.BaseQuantities
{

    public enum LengthType
    {
        Normal,
        Radius
    }

    public class Length : AnyQuantity
    {

        public LengthType LengthType { get; set; }

        public Length() : base(1) 
        {
            LengthType = LengthType.Normal;

        }

        public Length(int exponent) : base(exponent) 
        {
            LengthType = LengthType.Normal;
        }

        public Length(int exponent, LengthType lengthType)
            : base(exponent)
        {
            LengthType = lengthType;
        }

        public override QuantityDimension Dimension
        {
            get
            {


                QuantityDimension LengthDimension = new QuantityDimension();

                switch (LengthType)
                {
                    case LengthType.Normal:
                        LengthDimension.Length = new LengthDescriptor(Exponent,  0);
                        break;
                    case LengthType.Radius:
                        LengthDimension.Length = new LengthDescriptor(0,  Exponent);
                        break;
                }
                
                return LengthDimension;
            }
        }
    }
}
