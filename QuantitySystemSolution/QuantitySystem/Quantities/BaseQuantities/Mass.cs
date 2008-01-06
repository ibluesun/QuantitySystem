using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Mass : AnyQuantity
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
        public Mass(int exponent) : base(exponent) { }



        public override QuantityDimension Dimension
        {
            get
            {
                return new QuantityDimension(1, 0, 0) * Exponent;
            }
        }
    }
}
