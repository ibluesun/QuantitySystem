using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Mass<T> : AnyQuantity<T>
    {
        /// <summary>
        /// Create Mass object with dimension equals 1 M^1.
        /// </summary>
        public Mass() : base(1) 
        {
            
        }

        /// <summary>
        /// Create Mass object with the desired dimension.
        /// </summary>
        /// <param name="exponent">exponent of created Quantity</param>
        public Mass(float exponent) : base(exponent) { }



        public override QuantityDimension Dimension
        {
            get
            {
                return new QuantityDimension(1, 0, 0) * Exponent;
            }
        }


        public static implicit operator Mass<T>(T value)
        {
            Mass<T> Q = new Mass<T>();

            Q.Value = value;

            return Q;
        }

    }
}
