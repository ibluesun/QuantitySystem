using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Currency<T> : AnyQuantity<T>
    {
        /// <summary>
        /// Create Money object with dimension equals 1 M^1.
        /// </summary>
        public Currency() : base(1) 
        {
            
        }

        /// <summary>
        /// Create Mass object with the desired dimension.
        /// </summary>
        /// <param name="exponent">exponent of created Quantity</param>
        public Currency(float exponent) : base(exponent) { }


        private static QuantityDimension _Dimension = new QuantityDimension() 
        {   
            Currency = new DimensionDescriptors.CurrencyDescriptor(1)
        };

        public override QuantityDimension Dimension
        {
            get
            {
                return _Dimension * Exponent;
            }
        }


        public static implicit operator Currency<T>(T value)
        {
            Currency<T> Q = new Currency<T>();

            Q.Value = value;

            return Q;
        }

    }
}
