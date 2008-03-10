using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

using System.Collections.ObjectModel;


using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class DerivedQuantity : AnyQuantity
    {


        #region class instantiation
        private AnyQuantity[] InternalQuantities;


        public DerivedQuantity(int exponent, params AnyQuantity[] internalQuantities)
            : base(exponent)
        {
            InternalQuantities = internalQuantities;

        }

        public AnyQuantity[] GetInternalQuantities()
        {
            return InternalQuantities;
        }

        internal void SetInternalQuantities(AnyQuantity[] quantities)
        {
            InternalQuantities = quantities;
        }
        

        #endregion

        #region M L T Processing


        public override QuantityDimension Dimension
        {
            get
            {
                QuantityDimension QDTotal = new QuantityDimension();

                foreach (AnyQuantity  aq in InternalQuantities)
                {
                    QDTotal += aq.Dimension;
                }

                return QDTotal;
            }
        }

        public override BaseQuantity Invert()
        {
            List<AnyQuantity> lq = new List<AnyQuantity>();

            foreach (AnyQuantity qty in InternalQuantities)
            {
                lq.Add((AnyQuantity)qty.Invert());
            }

            DerivedQuantity dq = (DerivedQuantity) base.Invert();
            dq.SetInternalQuantities(lq.ToArray());
            return dq;
        }
        
        
        #endregion





        
    }
}
