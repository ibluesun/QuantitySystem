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

    public class Length<T> : AnyQuantity<T>
    {

        public LengthType LengthType { get; set; }

        public Length() : base(1) 
        {
            LengthType = LengthType.Normal;
        }

        public Length(float exponent)
            : base(exponent) 
        {
            LengthType = LengthType.Normal;
        }

        public Length(float exponent, LengthType lengthType)
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


        public static implicit operator Length<T>(T value)
        {
            Length<T> Q = new Length<T>();

            Q.Value = value;

            return Q;
        }

    }


    /// <summary>
    /// The Length but in Radius mode
    /// very usefull in differentiating of anlges and Angular quantities in general.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RadiusLength<T> : Length<T>
    {
        public RadiusLength() : base(1) 
        {
            LengthType = LengthType.Radius;
        }

        public RadiusLength(float exponent) : base(exponent) 
        {
            LengthType = LengthType.Radius;
        }

    }
}
