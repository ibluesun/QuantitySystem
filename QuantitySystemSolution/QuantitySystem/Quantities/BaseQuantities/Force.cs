using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Force : DerivedQuantity
    {
        public Force()
            : base(1, new Mass(), new Acceleration())
        {
        }

        public Force(int exponent)
            : base(exponent, new Mass(exponent), new Acceleration(exponent))
        {
        }



    }
}
